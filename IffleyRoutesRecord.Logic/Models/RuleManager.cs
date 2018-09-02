using IffleyRoutesRecord.Logic.DataAccess;
using IffleyRoutesRecord.Logic.DTOs.Received;
using IffleyRoutesRecord.Logic.DTOs.Sent;
using IffleyRoutesRecord.Logic.Entities;
using IffleyRoutesRecord.Logic.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace IffleyRoutesRecord.Logic.Models
{
    public class RuleManager : IRuleManager
    {
        private readonly IffleyRoutesRecordContext iffleyRoutesRecordContext;

        public RuleManager(IffleyRoutesRecordContext iffleyRoutesRecordContext)
        {
            this.iffleyRoutesRecordContext = iffleyRoutesRecordContext;
        }

        public ProblemRuleDto GetProblemRule(int ruleId)
        {
            var rule = iffleyRoutesRecordContext.GeneralRule.Single(ruleDbo => ruleDbo.Id == ruleId);
            return CreateProblemRuleDto(rule);
        }

        public IEnumerable<ProblemRuleDto> GetAllProblemRules()
        {
            return iffleyRoutesRecordContext.GeneralRule.Select(CreateProblemRuleDto);
        }

        public HoldRuleDto GetHoldRule(int ruleId)
        {
            var rule = iffleyRoutesRecordContext.HoldRule.Single(ruleDbo => ruleDbo.Id == ruleId);
            return CreateHoldRuleDto(rule);
        }

        public IEnumerable<HoldRuleDto> GetAllHoldRules()
        {
            return iffleyRoutesRecordContext.HoldRule.Select(CreateHoldRuleDto);
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
            AddNewRulesToDatabase(newRules, problemId);
            AddExistingRulesToDatabase(existingRuleIds, problemId);
        }

        private void AddExistingRulesToDatabase(IEnumerable<int> existingRuleIds, int problemId)
        {
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

        private void AddNewRulesToDatabase(IEnumerable<CreateProblemRuleDto> newRules, int problemId)
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
        }

        private ProblemRuleDto CreateProblemRuleDto(GeneralRule rule)
        {
            return new ProblemRuleDto()
            {
                ProblemRuleId = rule.Id,
                Name = rule.Name,
                Description = rule.Description
            };
        }

        private HoldRuleDto CreateHoldRuleDto(HoldRule rule)
        {
            return new HoldRuleDto()
            {
                HoldRuleId = rule.Id,
                Name = rule.Name,
                Description = rule.Description
            };
        }
    }
}
