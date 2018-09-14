using IffleyRoutesRecord.Logic.DTOs.Requests;
using IffleyRoutesRecord.Logic.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace IffleyRoutesRecord.Logic.Interfaces
{
    public interface IRuleManager
    {
        ProblemRuleResponse GetProblemRule(int ruleId);
        IEnumerable<ProblemRuleResponse> GetAllProblemRules();
        IEnumerable<HoldRuleResponse> GetHoldRules(int holdId, int problemId);
        IEnumerable<ProblemRuleResponse> GetProblemRules(int problemId);
        HoldRuleResponse GetHoldRule(int ruleId);
        IEnumerable<HoldRuleResponse> GetAllHoldRules();
    }
}