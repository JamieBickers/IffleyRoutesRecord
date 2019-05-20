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
    /// <summary>
    /// Reads problems
    /// </summary>
    public class ProblemReader
    {
        private readonly IffleyRoutesRecordContext repository;
        private readonly IMemoryCache cache;
        private readonly StyleSymbolManager styleSymbolManager;
        private readonly RuleManager ruleManager;
        private readonly HoldManager holdManager;
        private readonly IGradeManager gradeManager;

        public ProblemReader(IffleyRoutesRecordContext repository, IMemoryCache cache, StyleSymbolManager styleSymbolManager,
            RuleManager ruleManager, HoldManager holdManager, IGradeManager gradeManager)
        {
            this.repository = repository;
            this.cache = cache;
            this.styleSymbolManager = styleSymbolManager;
            this.ruleManager = ruleManager;
            this.holdManager = holdManager;
            this.gradeManager = gradeManager;
        }

        /// <summary>
        /// Gets the problem with the Given ID
        /// </summary>
        /// <param name="problemId">ID of the problem to get</param>
        /// <returns>The problem</returns>
        /// <exception cref="EntityNotFoundException"></exception>
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

            return problemResponse;
        }

        /// <summary>
        /// Gets all problems
        /// </summary>
        /// <returns>A list of all problems</returns>
        public IEnumerable<ProblemResponse> GetProblems()
        {
            if (!cache.TryRetrieveAllItems<ProblemResponse>(out var problems))
            {
                problems = IncludeAllData(repository.Problem)
                    .AsEnumerable()
                    .Select(problem => Mapper.Map(problem))
                    .ToList();

                cache.CacheListOfItems(problems, CacheItemPriority.Normal);
            }
            return problems;
        }

        /// <summary>
        /// Gets all unverified problems.
        /// </summary>
        /// <returns>A list of all unverified problems</returns>
        public IEnumerable<ProblemResponse> GetUnverifiedProblems()
        {
            var problems = IncludeAllData(repository.Problem)
            .Where(problem => problem.Verified)
            .AsEnumerable()
            .Select(problem => Mapper.Map(problem))
            .ToList();

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