using IffleyRoutesRecord.DTOs;
using IffleyRoutesRecord.Entities;
using IffleyRoutesRecord.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IffleyRoutesRecord.Logic
{
    public class HoldManager : IHoldManager
    {
        private readonly IffleyRoutesRecordContext iffleyRoutesRecordContext;
        private IRuleManager ruleManager;

        public HoldManager(IffleyRoutesRecordContext iffleyRoutesRecordContext, IRuleManager ruleManager)
        {
            this.iffleyRoutesRecordContext = iffleyRoutesRecordContext;
            this.ruleManager = ruleManager;
        }

        public IList<HoldDto> GetHoldsOnProblem(int problemId)
        {
            var problem = iffleyRoutesRecordContext
                .Problem
                .Include(problemDbo => problemDbo.ProblemHolds)
                .ThenInclude(problemHold => problemHold.Hold)
                .SingleOrDefault(p => p.Id == problemId);

            return problem.ProblemHolds
                .OrderBy(problemHold => problemHold.Position)
                .Select(problemHold => new HoldDto()
                {
                    HoldId = problemHold.HoldId,
                    Name = problemHold.Hold.Name,
                    IsStandingStartHold = problemHold.IsStandingStartHold,
                    HoldRules = ruleManager.GetHoldRules(problemHold.Hold.Id, problemId)
                })
                .ToList();
        }

        public void AddProblemHoldsToDatabase(IList<CreateHoldOnProblemDto> holds, int problemId)
        {
            for (int i = 0; i < holds.Count(); i++)
            {
                var hold = holds[i];
                var problemHoldDbo = new ProblemHold()
                {
                    HoldId = hold.HoldId,
                    ProblemId = problemId,
                    Position = i,
                    IsStandingStartHold = hold.IsStandingStartHold
                };

                iffleyRoutesRecordContext.ProblemHold.Add(problemHoldDbo);

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
                            ProblemHoldId = problemHoldDbo.Id
                        });
                    }
                }

                if (hold.ExistingHoldRuleIds != null)
                {
                    foreach (int ruleId in hold.ExistingHoldRuleIds)
                    {
                        iffleyRoutesRecordContext.ProblemHoldRule.Add(new ProblemHoldRule()
                        {
                            HoldRuleId = ruleId,
                            ProblemHoldId = problemHoldDbo.Id
                        });
                    }
                }
            }
        }
    }
}
