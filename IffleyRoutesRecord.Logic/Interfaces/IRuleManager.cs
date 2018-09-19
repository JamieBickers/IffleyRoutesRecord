using IffleyRoutesRecord.Models.DTOs.Responses;
using System.Collections.Generic;

namespace IffleyRoutesRecord.Logic.Interfaces
{
    /// <summary>
    /// Basic CRUD operations for rules
    /// </summary>
    public interface IRuleManager
    {
        /// <summary>
        /// Gets a rule for problems
        /// </summary>
        /// <param name="ruleId">ID of the rule</param>
        /// <returns>The rule</returns>
        /// <exception cref="EntityNotFoundException"></exception>
        ProblemRuleResponse GetProblemRule(int ruleId);

        /// <summary>
        /// Gets all rules on problems
        /// </summary>
        /// <returns>A list of all problem rules</returns>
        IEnumerable<ProblemRuleResponse> GetAllProblemRules();

        /// <summary>
        /// Gets a rule for holds
        /// </summary>
        /// <param name="ruleId">ID of the rule</param>
        /// <returns>The rule</returns>
        /// <exception cref="EntityNotFoundException"></exception>
        HoldRuleResponse GetHoldRule(int ruleId);

        /// <summary>
        /// Gets all rules on holds
        /// </summary>
        /// <returns>A list of all hold rules</returns>
        IEnumerable<HoldRuleResponse> GetAllHoldRules();
    }
}