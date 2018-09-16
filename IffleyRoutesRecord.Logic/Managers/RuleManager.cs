using IffleyRoutesRecord.Logic.DataAccess;
using IffleyRoutesRecord.Logic.DTOs.Responses;
using IffleyRoutesRecord.Logic.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;

namespace IffleyRoutesRecord.Logic.Managers
{
    public class RuleManager : IRuleManager
    {
        private readonly IffleyRoutesRecordContext repository;
        private readonly IMemoryCache cache;

        public RuleManager(IffleyRoutesRecordContext repository, IMemoryCache cache)
        {
            this.repository = repository;
            this.cache = cache;
        }

        public ProblemRuleResponse GetProblemRule(int ruleId)
        {
            if (cache.TryRetrieveItemWithId<ProblemRuleResponse>(ruleId, ruleFromCache => ruleFromCache.ProblemRuleId, out var problemRule))
            {
                return problemRule;
            }

            var rule = repository.GeneralRule.Single(ruleDbo => ruleDbo.Id == ruleId);
            return Mapper.Map(rule);
        }

        public IEnumerable<ProblemRuleResponse> GetAllProblemRules()
        {
            if (cache.TryRetrieveAllItems<ProblemRuleResponse>(out var rules))
            {
                return rules;
            }

            var ruleResponses = repository.GeneralRule.Select(Mapper.Map);
            cache.CacheListOfItems(ruleResponses, CacheItemPriority.Normal);

            return ruleResponses;
        }

        public HoldRuleResponse GetHoldRule(int ruleId)
        {
            if (cache.TryRetrieveItemWithId<HoldRuleResponse>(ruleId, ruleFromCache => ruleFromCache.HoldRuleId, out var holdRule))
            {
                return holdRule;
            }

            var rule = repository.HoldRule.Single(ruleDbo => ruleDbo.Id == ruleId);
            return Mapper.Map(rule);
        }

        public IEnumerable<HoldRuleResponse> GetAllHoldRules()
        {
            if (cache.TryRetrieveAllItems<HoldRuleResponse>(out var rules))
            {
                return rules;
            }

            var ruleResponses = repository.HoldRule.Select(Mapper.Map);
            cache.CacheListOfItems(ruleResponses, CacheItemPriority.Normal);

            return ruleResponses;
        }

        public IEnumerable<ProblemRuleResponse> GetProblemRules(int problemId)
        {
            return repository.ProblemRule
                .Where(rule => rule.ProblemId == problemId)
                .Select(Mapper.Map);
        }

        public IEnumerable<HoldRuleResponse> GetHoldRules(int holdId, int problemId)
        {
            return repository.ProblemHoldRule
                .Where(problemHoldRule => problemHoldRule.ProblemHold.ProblemId == problemId && problemHoldRule.ProblemHold.HoldId == holdId)
                .Select(problemHoldRule => new HoldRuleResponse()
                {
                    HoldRuleId = problemHoldRule.HoldRuleId,
                    Name = problemHoldRule.HoldRule.Name,
                    Description = problemHoldRule.HoldRule.Description
                });
        }
    }
}
