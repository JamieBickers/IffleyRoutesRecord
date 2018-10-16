using System;
using IffleyRoutesRecord.Logic.DataAccess;
using IffleyRoutesRecord.Logic.Exceptions;
using IffleyRoutesRecord.Logic.Interfaces;
using IffleyRoutesRecord.Logic.StaticHelpers;
using IffleyRoutesRecord.Models.DTOs.Requests;
using IffleyRoutesRecord.Models.DTOs.Responses;
using IffleyRoutesRecord.Models.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace IffleyRoutesRecord.Logic.Managers
{
    public class ProblemCreator : IProblemCreator
    {
        private readonly IffleyRoutesRecordContext repository;
        private readonly IMemoryCache cache;
        private readonly IProblemReader problemReader;
        private readonly IProblemRequestValidator validator;

        public ProblemCreator(IffleyRoutesRecordContext repository, IMemoryCache cache,
            IProblemReader problemReader, IProblemRequestValidator validator)
        {
            this.repository = repository;
            this.cache = cache;
            this.problemReader = problemReader;
            this.validator = validator;
        }

        public ProblemResponse CreateProblem(CreateProblemRequest problem)
        {
            if (problem is null)
            {
                throw new ArgumentNullException(nameof(problem));
            }

            validator.Validate(problem);

            var problemDbo = AddProblemToDatabase(problem);
            repository.SaveChanges();
            return UpdateCache(problemDbo);
        }

        private ProblemResponse UpdateCache(Problem problemDbo)
        {
            // If an unexpected error occurs we want to clear the relevant cache to maintain data integrity
            try
            {
                var problemResponse = problemReader.GetProblem(problemDbo.Id);
                cache.AddItemToCachedList(problemResponse);
                return problemResponse;
            }
            // Don't want to return an EntityNotFoundException exception to the users as it is missleading
            catch (EntityNotFoundException exception)
            {
                throw new EntityCreationException(string.Empty, exception);
            }
            catch
            {
                cache.RemoveCachedListOfItems<ProblemResponse>();
                throw;
            }
            finally
            {
                cache.RemoveCachedListOfItems<ProblemRuleResponse>();
                cache.RemoveCachedListOfItems<HoldRuleResponse>();
            }
        }

        private Problem AddProblemToDatabase(CreateProblemRequest problem)
        {
            var problemDbo = Mapper.Map(problem);
            repository.Problem.Add(problemDbo);
            return problemDbo;
        }
    }
}
