using IffleyRoutesRecord.Logic.DataAccess;
using IffleyRoutesRecord.Logic.DTOs.Requests;
using IffleyRoutesRecord.Logic.DTOs.Responses;
using IffleyRoutesRecord.Logic.Entities;
using IffleyRoutesRecord.Logic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IffleyRoutesRecord.Logic.Managers
{
    public class HoldManager : IHoldManager
    {
        private readonly IffleyRoutesRecordContext iffleyRoutesRecordContext;
        private readonly IMemoryCache cache;
        private IRuleManager ruleManager;

        public HoldManager(IffleyRoutesRecordContext iffleyRoutesRecordContext, IMemoryCache cache, IRuleManager ruleManager)
        {
            this.iffleyRoutesRecordContext = iffleyRoutesRecordContext;
            this.cache = cache;
            this.ruleManager = ruleManager;
        }

        public HoldResponse GetHold(int holdId)
        {
            if (cache.TryRetrieveItemWithId<HoldResponse>(holdId, cachedHoldResponse => cachedHoldResponse.HoldId, out var holdResponse))
            {
                return holdResponse;
            }

            var hold = iffleyRoutesRecordContext.Hold.Single(holdDbo => holdDbo.Id == holdId);
            return CreateHoldDto(hold);
        }

        public IEnumerable<HoldResponse> GetHolds()
        {
            if (cache.TryRetrieveAllItems<HoldResponse>(out var holdResponses))
            {
                return holdResponses;
            }

            var holds = iffleyRoutesRecordContext.Hold.Select(CreateHoldDto);
            cache.CacheListOfItems(holds, CacheItemPriority.High);

            return holds;
        }

        public IList<HoldOnProblemResponse> GetHoldsOnProblem(int problemId)
        {
            var problem = iffleyRoutesRecordContext
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

        public void AddProblemHoldsToDatabase(IList<CreateHoldOnProblemRequest> holds, int problemId)
        {
            if (holds is null)
            {
                throw new ArgumentNullException(nameof(holds));
            }

            ValidateCreateHoldsOnProblemRequest(holds, problemId);
            for (int i = 0; i < holds.Count(); i++)
            {
                var hold = holds[i];

                int problemHoldId = AddProblemHoldToDatabase(problemId, i, hold);

                AddNewRulesToDatabase(hold, problemHoldId);
                AddExistingRulesToDatabase(hold, problemHoldId);
            }
        }

        private void ValidateCreateHoldsOnProblemRequest(IEnumerable<CreateHoldOnProblemRequest> holds, int problemId)
        {
            if (holds is null)
            {
                throw new ArgumentNullException(nameof(holds));
            }

            iffleyRoutesRecordContext.Problem.VerifyEntityWithIdExists(problemId);

            foreach (var hold in holds)
            {
                iffleyRoutesRecordContext.Hold.VerifyEntityWithIdExists(hold.HoldId);

                if (hold.ExistingHoldRuleIds != null)
                {
                    foreach (int ruleId in hold.ExistingHoldRuleIds)
                    {
                        iffleyRoutesRecordContext.HoldRule.VerifyEntityWithIdExists(ruleId);
                    }
                }

                if (hold.NewHoldRules != null)
                {
                    foreach (var newRule in hold.NewHoldRules)
                    {
                        iffleyRoutesRecordContext.HoldRule.VerifyEntityWithNameDoesNotExists(newRule.Name);
                    }
                }
            }
        }

        private void AddExistingRulesToDatabase(CreateHoldOnProblemRequest hold, int problemHoldId)
        {
            if (hold is null)
            {
                throw new ArgumentNullException(nameof(hold));
            }

            if (hold.ExistingHoldRuleIds != null)
            {
                foreach (int ruleId in hold.ExistingHoldRuleIds)
                {
                    iffleyRoutesRecordContext.ProblemHoldRule.Add(new ProblemHoldRule()
                    {
                        HoldRuleId = ruleId,
                        ProblemHoldId = problemHoldId
                    });
                }
            }
        }

        private void AddNewRulesToDatabase(CreateHoldOnProblemRequest hold, int problemHoldId)
        {
            if (hold is null)
            {
                throw new ArgumentNullException(nameof(hold));
            }

            if (hold.NewHoldRules != null)
            {
                foreach (var rule in hold.NewHoldRules)
                {
                    var holdRuleDbo = new HoldRule()
                    {
                        Name = rule.Name,
                        Description = rule.Description
                    };

                    iffleyRoutesRecordContext.HoldRule.Add(holdRuleDbo);

                    iffleyRoutesRecordContext.ProblemHoldRule.Add(new ProblemHoldRule()
                    {
                        HoldRuleId = holdRuleDbo.Id,
                        ProblemHoldId = problemHoldId
                    });
                }
            }
        }

        private int AddProblemHoldToDatabase(int problemId, int position, CreateHoldOnProblemRequest hold)
        {
            if (hold is null)
            {
                throw new ArgumentNullException(nameof(hold));
            }

            var problemHoldDbo = new ProblemHold()
            {
                HoldId = hold.HoldId,
                ProblemId = problemId,
                Position = position,
                IsStandingStartHold = hold.IsStandingStartHold
            };

            iffleyRoutesRecordContext.ProblemHold.Add(problemHoldDbo);
            return problemHoldDbo.Id;
        }

        private HoldResponse CreateHoldDto(Hold hold)
        {
            if (hold is null)
            {
                throw new ArgumentNullException(nameof(hold));
            }

            return new HoldResponse()
            {
                HoldId = hold.Id,
                Name = hold.Name,
            };
        }
    }
}
