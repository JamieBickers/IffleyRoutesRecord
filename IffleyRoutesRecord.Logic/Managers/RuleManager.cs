using IffleyRoutesRecord.Logic.DataAccess;
using IffleyRoutesRecord.Logic.DTOs.Requests;
using IffleyRoutesRecord.Logic.DTOs.Responses;
using IffleyRoutesRecord.Logic.Entities;
using IffleyRoutesRecord.Logic.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
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
            return CreateProblemRuleResponse(rule);
        }

        public IEnumerable<ProblemRuleResponse> GetAllProblemRules()
        {
            if (cache.TryRetrieveAllItems<ProblemRuleResponse>(out var rules))
            {
                return rules;
            }

            var ruleResponses = repository.GeneralRule.Select(CreateProblemRuleResponse);
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
            return CreateHoldRuleResponse(rule);
        }

        public IEnumerable<HoldRuleResponse> GetAllHoldRules()
        {
            if (cache.TryRetrieveAllItems<HoldRuleResponse>(out var rules))
            {
                return rules;
            }

            var ruleResponses = repository.HoldRule.Select(CreateHoldRuleResponse);
            cache.CacheListOfItems(ruleResponses, CacheItemPriority.Normal);

            return ruleResponses;
        }

        public IEnumerable<ProblemRuleResponse> GetProblemRules(int problemId)
        {
            return repository.ProblemRule
                .Where(rule => rule.ProblemId == problemId).Select(rule => new ProblemRuleResponse()
                {
                    ProblemRuleId = rule.Id,
                    Name = rule.GeneralRule.Name,
                    Description = rule.GeneralRule.Description
                });
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

        private ProblemRuleResponse CreateProblemRuleResponse(GeneralRule rule)
        {
            if (rule is null)
            {
                throw new ArgumentNullException(nameof(rule));
            }

            return new ProblemRuleResponse()
            {
                ProblemRuleId = rule.Id,
                Name = rule.Name,
                Description = rule.Description
            };
        }

        private HoldRuleResponse CreateHoldRuleResponse(HoldRule rule)
        {
            if (rule is null)
            {
                throw new ArgumentNullException(nameof(rule));
            }

            return new HoldRuleResponse()
            {
                HoldRuleId = rule.Id,
                Name = rule.Name,
                Description = rule.Description
            };
        }
    }
}
