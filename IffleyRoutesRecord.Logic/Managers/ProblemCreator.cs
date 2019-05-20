using System;
using System.Linq;
using IffleyRoutesRecord.Logic.DataAccess;
using IffleyRoutesRecord.Logic.Exceptions;
using IffleyRoutesRecord.Logic.Interfaces;
using IffleyRoutesRecord.Logic.StaticHelpers;
using IffleyRoutesRecord.Logic.Validators;
using IffleyRoutesRecord.Models.DTOs.Requests;
using IffleyRoutesRecord.Models.DTOs.Responses;
using IffleyRoutesRecord.Models.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace IffleyRoutesRecord.Logic.Managers
{
    /// <summary>
    /// Creates problems
    /// </summary>
    public class ProblemCreator
    {
        private readonly IffleyRoutesRecordContext repository;
        private readonly IMemoryCache cache;
        private readonly ProblemReader problemReader;
        private readonly ProblemRequestValidator validator;

        public ProblemCreator(IffleyRoutesRecordContext repository, IMemoryCache cache,
            ProblemReader problemReader, ProblemRequestValidator validator)
        {
            this.repository = repository;
            this.cache = cache;
            this.problemReader = problemReader;
            this.validator = validator;
        }

        /// <summary>
        /// Creates a problem
        /// </summary>
        /// <param name="problem">Problem to create</param>
        /// <returns>The created problem</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="EntityNotFoundException"></exception>
        /// <exception cref="EntityWithNameAlreadyExistsException"></exception>
        /// <exception cref="InternalEntityNotFoundException"></exception>
        public ProblemResponse CreateUnverifiedProblem(CreateProblemRequest problem, string userEmail)
        {
            if (problem is null)
            {
                throw new ArgumentNullException(nameof(problem));
            }

            validator.Validate(problem);

            var problemDbo = AddUnverifiedProblemToDatabase(problem, userEmail);
            repository.SaveChanges();

            try
            {
                return problemReader.GetProblem(problemDbo.Id);
            }
            catch (EntityNotFoundException exception)
            {
                throw new EntityCreationException(string.Empty, exception);
            }
        }

        /// <summary>
        /// Sets the verified flag on a problem to true.
        /// </summary>
        /// <param name="problemId">ID of the problem to set the verified flag on</param>
        /// <exception cref="EntityNotFoundException"></exception>
        /// <exception cref="InternalEntityNotFoundException"></exception>
        public ProblemResponse VerifyProblem(int problemId)
        {
            repository.Problem.VerifyEntityWithIdExists(problemId);
            repository.Problem.FirstOrDefault(problem => problem.Id == problemId).Verified = true;
            repository.SaveChanges();
            return UpdateCache(problemId);
        }

        private ProblemResponse UpdateCache(int problemId)
        {
            // If an unexpected error occurs we want to clear the relevant cache to maintain data integrity
            try
            {
                var problemResponse = problemReader.GetProblem(problemId);
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

        private Problem AddUnverifiedProblemToDatabase(CreateProblemRequest problem, string userEmail)
        {
            var problemDbo = Mapper.Map(problem);
            problemDbo.Verified = false;
            problemDbo.LoggedBy = userEmail;
            repository.Problem.Add(problemDbo);
            return problemDbo;
        }
    }
}
