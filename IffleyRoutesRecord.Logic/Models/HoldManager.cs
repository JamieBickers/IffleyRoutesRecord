using IffleyRoutesRecord.Logic.DataAccess;
using IffleyRoutesRecord.Logic.DTOs.Received;
using IffleyRoutesRecord.Logic.DTOs.Sent;
using IffleyRoutesRecord.Logic.Entities;
using IffleyRoutesRecord.Logic.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace IffleyRoutesRecord.Logic.Models
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

        public HoldDto GetHold(int holdId)
        {
            var hold = iffleyRoutesRecordContext.Hold.Single(holdDbo => holdDbo.Id == holdId);
            return CreateHoldDto(hold);
        }

        public IEnumerable<HoldDto> GetHolds()
        {
            return iffleyRoutesRecordContext.Hold.Select(CreateHoldDto);
        }

        public IList<HoldOnProblemDto> GetHoldsOnProblem(int problemId)
        {
            var problem = iffleyRoutesRecordContext
                .Problem
                .Include(problemDbo => problemDbo.ProblemHolds)
                .ThenInclude(problemHold => problemHold.Hold)
                .SingleOrDefault(p => p.Id == problemId);

            return problem.ProblemHolds
                .OrderBy(problemHold => problemHold.Position)
                .Select(problemHold => new HoldOnProblemDto()
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

                int problemHoldId = AddProblemHoldToDatabase(problemId, i, hold);

                AddNewRulesToDatabase(hold, problemHoldId);
                AddExistingRulesToDatabase(hold, problemHoldId);
            }
        }

        private void AddExistingRulesToDatabase(CreateHoldOnProblemDto hold, int problemHoldId)
        {
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

        private void AddNewRulesToDatabase(CreateHoldOnProblemDto hold, int problemHoldId)
        {
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

        private int AddProblemHoldToDatabase(int problemId, int position, CreateHoldOnProblemDto hold)
        {
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

        private HoldDto CreateHoldDto(Hold hold)
        {
            return new HoldDto()
            {
                HoldId = hold.Id,
                Name = hold.Name,
            };
        }
    }
}
