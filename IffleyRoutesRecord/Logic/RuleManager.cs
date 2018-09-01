using IffleyRoutesRecord.DTOs;
using IffleyRoutesRecord.Entities;
using IffleyRoutesRecord.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace IffleyRoutesRecord.Logic
{
    public class RuleManager : IRuleManager
    {
        private readonly IffleyRoutesRecordContext iffleyRoutesRecordContext;

        public RuleManager(IffleyRoutesRecordContext iffleyRoutesRecordContext)
        {
            this.iffleyRoutesRecordContext = iffleyRoutesRecordContext;
        }

        public IEnumerable<ProblemRuleDto> GetProblemRules(int problemId)
        {
            return iffleyRoutesRecordContext.ProblemRule
                .Where(rule => rule.ProblemId == problemId).Select(rule => new ProblemRuleDto()
                {
                    ProblemRuleId = rule.Id,
                    Name = rule.GeneralRule.Name,
                    Description = rule.GeneralRule.Description
                });
        }

        public IEnumerable<HoldRuleDto> GetHoldRules(int holdId, int problemId)
        {
            return iffleyRoutesRecordContext.ProblemHoldRule
                .Where(problemHoldRule => problemHoldRule.ProblemHold.ProblemId == problemId && problemHoldRule.ProblemHold.HoldId == holdId)
                .Select(problemHoldRule => new HoldRuleDto()
                {
                    HoldRuleId = problemHoldRule.HoldRuleId,
                    Name = problemHoldRule.HoldRule.Name,
                    Description = problemHoldRule.HoldRule.Description
                });
        }

        public void AddRulesToDatabase(IEnumerable<CreateProblemRuleDto> newRules, IEnumerable<int> existingRuleIds, int problemId)
        {
            if (newRules != null)
            {
                foreach (var rule in newRules)
                {
                    var generalRuleDbo = new GeneralRule()
                    {
                        Name = rule.Name,
                        Description = rule.Description
                    };

                    iffleyRoutesRecordContext.GeneralRule.Add(generalRuleDbo);

                    iffleyRoutesRecordContext.ProblemRule.Add(new ProblemRule()
                    {
                        GeneralRuleId = generalRuleDbo.Id,
                        ProblemId = problemId
                    });
                }
            }

            if (existingRuleIds != null)
            {
                foreach (int ruleId in existingRuleIds)
                {
                    iffleyRoutesRecordContext.ProblemRule.Add(new ProblemRule()
                    {
                        GeneralRuleId = ruleId,
                        ProblemId = problemId
                    });
                }
            }
        }
    }
}
