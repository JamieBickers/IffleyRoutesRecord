using IffleyRoutesRecord.Logic.DTOs.Received;
using IffleyRoutesRecord.Logic.DTOs.Sent;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace IffleyRoutesRecord.Logic.Interfaces
{
    public interface IRuleManager
    {
        ProblemRuleDto GetProblemRule(int ruleId);
        IEnumerable<ProblemRuleDto> GetAllProblemRules();
        IEnumerable<HoldRuleDto> GetHoldRules(int holdId, int problemId);
        IEnumerable<ProblemRuleDto> GetProblemRules(int problemId);
        void AddRulesToDatabase(IEnumerable<CreateProblemRuleDto> newRules, IEnumerable<int> existingRuleIds, int problemId);
        HoldRuleDto GetHoldRule(int ruleId);
        IEnumerable<HoldRuleDto> GetAllHoldRules();
    }
}