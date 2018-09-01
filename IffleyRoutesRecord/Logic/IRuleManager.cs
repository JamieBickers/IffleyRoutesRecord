using System.Collections.Generic;
using IffleyRoutesRecord.DTOs;

namespace IffleyRoutesRecord.Logic
{
    public interface IRuleManager
    {
        IEnumerable<HoldRuleDto> GetHoldRules(int holdId, int problemId);
        IEnumerable<ProblemRuleDto> GetProblemRules(int problemId);
        void AddRulesToDatabase(IEnumerable<CreateProblemRuleDto> newRules, IEnumerable<int> existingRuleIds, int problemId);
    }
}