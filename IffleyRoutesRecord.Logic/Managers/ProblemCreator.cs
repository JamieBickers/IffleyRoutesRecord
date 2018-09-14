using System;
using IffleyRoutesRecord.Logic.DataAccess;
using IffleyRoutesRecord.Logic.DTOs.Requests;
using IffleyRoutesRecord.Logic.DTOs.Responses;
using IffleyRoutesRecord.Logic.Entities;
using IffleyRoutesRecord.Logic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace IffleyRoutesRecord.Logic.Managers
{
    public class ProblemCreator : IProblemCreator
    {
        private readonly IffleyRoutesRecordContext iffleyRoutesRecordContext;
        private readonly IMemoryCache cache;
        private readonly IStyleSymbolManager styleSymbolManager;
        private readonly IRuleManager ruleManager;
        private readonly IHoldManager holdManager;
        private readonly IGradeManager gradeManager;
        private readonly IProblemReader problemReader;

        public ProblemCreator(IffleyRoutesRecordContext iffleyRoutesRecordContext, IMemoryCache cache, IStyleSymbolManager styleSymbolManager,
            IRuleManager ruleManager, IHoldManager holdManager, IGradeManager gradeManager, IProblemReader problemReader)
        {
            this.iffleyRoutesRecordContext = iffleyRoutesRecordContext;
            this.cache = cache;
            this.styleSymbolManager = styleSymbolManager;
            this.ruleManager = ruleManager;
            this.holdManager = holdManager;
            this.gradeManager = gradeManager;
            this.problemReader = problemReader;
        }

        public ProblemResponse CreateProblem(CreateProblemRequest problem)
        {
            if (problem is null)
            {
                throw new ArgumentNullException(nameof(problem));
            }

            ValidateCreateProblemRequestion(problem);

            var problemDbo = AddProblemToDatabase(problem);
            ruleManager.AddRulesToDatabase(problem.NewRules, problem.ExistingRuleIds, problemDbo.Id);
            holdManager.AddProblemHoldsToDatabase(problem.Holds, problemDbo.Id);
            styleSymbolManager.AddProblemStyleSymbolsToDatabase(problem.StyleSymbolIds, problemDbo.Id);

            iffleyRoutesRecordContext.SaveChanges();

            try
            {
                var problemResponse = problemReader.GetProblem(problemDbo.Id);
                cache.AddItemToCachedList(problemResponse);
                return problemResponse;
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

        private void ValidateCreateProblemRequestion(CreateProblemRequest problem)
        {
            if (problem is null)
            {
                throw new ArgumentNullException(nameof(problem));
            }

            iffleyRoutesRecordContext.Problem.VerifyEntityWithNameDoesNotExists(problem.Name);

            VerifyGradeExists(problem.TechGradeId, iffleyRoutesRecordContext.TechGrade);
            VerifyGradeExists(problem.BGradeId, iffleyRoutesRecordContext.BGrade);
            VerifyGradeExists(problem.PoveyGradeId, iffleyRoutesRecordContext.PoveyGrade);
            VerifyGradeExists(problem.FurlongGradeId, iffleyRoutesRecordContext.FurlongGrade);
        }

        private void VerifyGradeExists<TGrade>(int? gradeId, DbSet<TGrade> grades) where TGrade : BaseEntity
        {
            if (grades is null)
            {
                throw new ArgumentNullException(nameof(grades));
            }

            if (gradeId is null)
            {
                return;
            }
            else
            {
                grades.VerifyEntityWithIdExists(gradeId.Value);
            }
        }

        private Problem AddProblemToDatabase(CreateProblemRequest problem)
        {
            if (problem is null)
            {
                throw new ArgumentNullException(nameof(problem));
            }

            var problemDbo = new Problem()
            {
                Name = problem.Name,
                Description = problem.Description,
                TechGradeId = problem.TechGradeId,
                BGradeId = problem.BGradeId,
                PoveyGradeId = problem.PoveyGradeId,
                FurlongGradeId = problem.FurlongGradeId,
                DateSet = problem.DateSet,
                FirstAscent = problem.FirstAscent
            };

            iffleyRoutesRecordContext.Problem.Add(problemDbo);
            return problemDbo;
        }
    }
}
