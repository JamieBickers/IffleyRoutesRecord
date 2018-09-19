using IffleyRoutesRecord.Logic.DataAccess;
using IffleyRoutesRecord.Logic.Exceptions;
using IffleyRoutesRecord.Logic.Interfaces;
using IffleyRoutesRecord.Logic.StaticHelpers;
using IffleyRoutesRecord.Models.DTOs.Responses;
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

            var rule = repository.GeneralRule.SingleOrDefault(ruleDbo => ruleDbo.Id == ruleId);

            if (rule is null)
            {
                throw new EntityNotFoundException($"No problem rule with ID {ruleId} was found.");
            }

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

            var rule = repository.HoldRule.SingleOrDefault(ruleDbo => ruleDbo.Id == ruleId);

            if (rule is null)
            {
                throw new EntityNotFoundException($"No hold rule with ID {ruleId} was found.");
            }

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
    }
}
