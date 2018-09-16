using IffleyRoutesRecord.Logic.DataAccess;
using IffleyRoutesRecord.Logic.DTOs.Responses;
using IffleyRoutesRecord.Logic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;

namespace IffleyRoutesRecord.Logic.Managers
{
    public class HoldManager : IHoldManager
    {
        private readonly IffleyRoutesRecordContext repository;
        private readonly IMemoryCache cache;
        private IRuleManager ruleManager;

        public HoldManager(IffleyRoutesRecordContext repository, IMemoryCache cache, IRuleManager ruleManager)
        {
            this.repository = repository;
            this.cache = cache;
            this.ruleManager = ruleManager;
        }

        public HoldResponse GetHold(int holdId)
        {
            if (cache.TryRetrieveItemWithId<HoldResponse>(holdId, cachedHoldResponse => cachedHoldResponse.HoldId, out var holdResponse))
            {
                return holdResponse;
            }

            var hold = repository.Hold.Single(holdDbo => holdDbo.Id == holdId);
            return Mapper.Map(hold);
        }

        public IEnumerable<HoldResponse> GetHolds()
        {
            if (cache.TryRetrieveAllItems<HoldResponse>(out var holdResponses))
            {
                return holdResponses;
            }

            var holds = repository.Hold.Select(Mapper.Map);
            cache.CacheListOfItems(holds, CacheItemPriority.High);

            return holds;
        }

        public IList<HoldOnProblemResponse> GetHoldsOnProblem(int problemId)
        {
            var problem = repository
                .Problem
                .Include(problemDbo => problemDbo.ProblemHolds)
                .ThenInclude(problemHold => problemHold.Hold)
                .SingleOrDefault(p => p.Id == problemId);

            return problem.ProblemHolds
                .OrderBy(problemHold => problemHold.Position)
                .Select(problemHold => new HoldOnProblemResponse()
                {
                    HoldId = problemHold.HoldId,
                    Name = problemHold.Hold.Name,
                    IsStandingStartHold = problemHold.IsStandingStartHold,
                    HoldRules = ruleManager.GetHoldRules(problemHold.Hold.Id, problemId).ToList()
                })
                .ToList();
        }
    }
}
