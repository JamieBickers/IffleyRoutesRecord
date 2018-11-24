using IffleyRoutesRecord.Logic.DataAccess;
using IffleyRoutesRecord.Logic.Exceptions;
using IffleyRoutesRecord.Logic.Interfaces;
using IffleyRoutesRecord.Logic.StaticHelpers;
using IffleyRoutesRecord.Models.DTOs.Responses;
using IffleyRoutesRecord.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IGlobalGradeAssigner globalGradeAssigner;

        public ProblemReader(IffleyRoutesRecordContext repository, IMemoryCache cache, IStyleSymbolManager styleSymbolManager,
            IRuleManager ruleManager, IHoldManager holdManager, IGradeManager gradeManager, IGlobalGradeAssigner globalGradeAssigner)
        {
            this.repository = repository;
            this.cache = cache;
            this.styleSymbolManager = styleSymbolManager;
            this.ruleManager = ruleManager;
            this.holdManager = holdManager;
            this.gradeManager = gradeManager;
            this.globalGradeAssigner = globalGradeAssigner;
        }

        public ProblemResponse GetProblem(int problemId)
        {
            if (!cache.TryRetrieveItemWithId<ProblemResponse>(problemId, problem => problem.ProblemId, out var problemResponse))
            {
                var problemDbo = IncludeAllData(repository.Problem).SingleOrDefault(route => route.Id == problemId);

                if (problemDbo is null)
                {
                    throw new EntityNotFoundException($"No problem with ID {problemId} was found.");
                }

                problemResponse = Mapper.Map(problemDbo);
            }

            globalGradeAssigner.AssignGlobalGrade(problemResponse);
            return problemResponse;
        }

        public IEnumerable<ProblemResponse> GetProblems()
        {
            if (!cache.TryRetrieveAllItems<ProblemResponse>(out var problems))
            {
                problems = IncludeAllData(repository.Problem)
                    .AsEnumerable()
                    .Select(problem => Mapper.Map(problem));

                cache.CacheListOfItems(problems, CacheItemPriority.Normal);
            }

            globalGradeAssigner.AssignGlobalGrades(problems);
            return problems;
        }

        private static IQueryable<Problem> IncludeAllData(IQueryable<Problem> problems)
        {
            return problems
                .Include(problem => problem.TechGrade)
                .Include(problem => problem.BGrade)
                .Include(problem => problem.PoveyGrade)
                .Include(problem => problem.FurlongGrade)
                .Include(problem => problem.ProblemHolds)
                    .ThenInclude(problemHold => problemHold.Hold)
                .Include(problem => problem.ProblemHolds)
                    .ThenInclude(problemHold => problemHold.ProblemHoldRules)
                        .ThenInclude(problemHold => problemHold.HoldRule)
                .Include(problem => problem.ProblemRules)
                    .ThenInclude(problemRule => problemRule.GeneralRule)
                .Include(problem => problem.ProblemStyleSymbols)
                    .ThenInclude(problemStyleSymbol => problemStyleSymbol.StyleSymbol);
        }
    }
}