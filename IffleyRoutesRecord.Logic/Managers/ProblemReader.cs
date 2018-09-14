using IffleyRoutesRecord.Logic.DataAccess;
using IffleyRoutesRecord.Logic.DTOs.Responses;
using IffleyRoutesRecord.Logic.Entities;
using IffleyRoutesRecord.Logic.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IffleyRoutesRecord.Logic.Managers
{
    public class ProblemReader : IProblemReader
    {
        private readonly IffleyRoutesRecordContext repository;
        private readonly IMemoryCache cache;
        private readonly IStyleSymbolManager styleSymbolManager;
        private readonly IRuleManager ruleManager;
        private readonly IHoldManager holdManager;
        private readonly IGradeManager gradeManager;

        public ProblemReader(IffleyRoutesRecordContext repository, IMemoryCache cache, IStyleSymbolManager styleSymbolManager,
            IRuleManager ruleManager, IHoldManager holdManager, IGradeManager gradeManager)
        {
            this.repository = repository;
            this.cache = cache;
            this.styleSymbolManager = styleSymbolManager;
            this.ruleManager = ruleManager;
            this.holdManager = holdManager;
            this.gradeManager = gradeManager;
        }

        public ProblemResponse GetProblem(int problemId)
        {
            if (cache.TryRetrieveItemWithId<ProblemResponse>(problemId, problem => problem.ProblemId, out var problemResponse))
            {
                return problemResponse;
            }

            var problemDbo = repository.Problem.SingleOrDefault(route => route.Id == problemId);

            if (problemDbo is null)
            {
                return null;
            }

            problemResponse = CreateProblemResponse(problemDbo);

            return problemResponse;
        }

        public IEnumerable<ProblemResponse> GetProblems()
        {
            if (cache.TryRetrieveAllItems<ProblemResponse>(out var problems))
            {
                return problems;
            }

            problems = repository
                .Problem
                .AsEnumerable()
                .Select(problem => CreateProblemResponse(problem));

            cache.CacheListOfItems(problems, CacheItemPriority.Normal);

            return problems;
        }

        //TODO: Simplify?
        private ProblemResponse CreateProblemResponse(Problem problem)
        {
            if (problem is null)
            {
                throw new ArgumentNullException(nameof(problem));
            }

            return new ProblemResponse()
            {
                ProblemId = problem.Id,
                Name = problem.Name,
                Description = problem.Description,
                DateSet = problem.DateSet,
                FirstAscent = problem.FirstAscent,
                TechGrade = gradeManager.GetTechGradeOnProblem(problem.Id),
                BGrade = gradeManager.GetBGradeOnProblem(problem.Id),
                PoveyGrade = gradeManager.GetPoveyGradeOnProblem(problem.Id),
                FurlongGrade = gradeManager.GetFurlongGradeOnProblem(problem.Id),
                Holds = holdManager.GetHoldsOnProblem(problem.Id),
                Rules = ruleManager.GetProblemRules(problem.Id).ToList(),
                StyleSymbols = styleSymbolManager.GetStyleSymbolsOnProblem(problem.Id).ToList()
            };
        }
    }
}