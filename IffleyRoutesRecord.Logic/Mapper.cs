using IffleyRoutesRecord.Logic.DTOs.Requests;
using IffleyRoutesRecord.Logic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IffleyRoutesRecord.Logic
{
    /// <summary>
    /// Maps between database entities and DTOs
    /// </summary>
    internal static class Mapper
    {
        /// <summary>
        /// Create a Problem entity from a CreateProblemRequest dto
        /// </summary>
        /// <param name="problem">The DTO to map from</param>
        /// <returns>The corresponding Problem entity</returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static Problem Map(CreateProblemRequest problem)
        {
            if (problem is null)
            {
                throw new ArgumentNullException(nameof(problem));
            }

            var problemDbo = new Problem()
            {
                Name = problem.Name,
                Description = problem.Description,
                DateSet = problem.DateSet,
                FirstAscent = problem.FirstAscent,
                TechGradeId = problem.TechGradeId,
                BGradeId = problem.BGradeId,
                PoveyGradeId = problem.PoveyGradeId,
                FurlongGradeId = problem.FurlongGradeId
            };

            if (problem.StyleSymbolIds != null)
            {
                AddStyleSymbolsToProblem(problem.StyleSymbolIds, problemDbo);
            }

            problemDbo.ProblemRules = CreateProblemRuleDbos(problem, problemDbo).ToList();
            problemDbo.ProblemHolds = problem.Holds.Select((hold, index) => CreateProblemHoldDbo(hold, index, problemDbo)).ToList();

            return problemDbo;
        }

        private static IEnumerable<ProblemRule> CreateProblemRuleDbos(CreateProblemRequest problem, Problem problemDbo)
        {
            if (problem is null)
            {
                throw new ArgumentNullException(nameof(problem));
            }

            if (problemDbo is null)
            {
                throw new ArgumentNullException(nameof(problemDbo));
            }

            var problemRules = new List<ProblemRule>();

            if (problem.ExistingRuleIds != null)
            {
                problemRules.AddRange(
                    problem.ExistingRuleIds
                    .Select(ruleId => new ProblemRule()
                    {
                        Problem = problemDbo,
                        GeneralRuleId = ruleId
                    }));
            }

            if (problem.NewRules != null)
            {
                problemRules.AddRange(
                    problem.NewRules
                    .Select(newRule => Map(newRule, problemDbo)));
            }

            return problemRules;
        }

        private static void AddStyleSymbolsToProblem(IEnumerable<int> styleSymbolIds, Problem problemDbo)
        {
            if (styleSymbolIds is null)
            {
                throw new ArgumentNullException(nameof(styleSymbolIds));
            }

            if (problemDbo is null)
            {
                throw new ArgumentNullException(nameof(problemDbo));
            }

            problemDbo.ProblemStyleSymbols = styleSymbolIds
                .Select(styleSymbolId => new ProblemStyleSymbol()
                {
                    Problem = problemDbo,
                    StyleSymbolId = styleSymbolId
                })
                .ToList();
        }

        private static ProblemRule Map(CreateProblemRuleRequest newRule, Problem problemDbo)
        {
            if (newRule is null)
            {
                throw new ArgumentNullException(nameof(newRule));
            }

            if (problemDbo is null)
            {
                throw new ArgumentNullException(nameof(problemDbo));
            }

            return new ProblemRule()
            {
                Problem = problemDbo,
                GeneralRule = new GeneralRule()
                {
                    Name = newRule.Name,
                    Description = newRule.Description
                }
            };
        }

        private static ProblemHold CreateProblemHoldDbo(CreateHoldOnProblemRequest hold, int index, Problem problemDbo)
        {
            if (hold is null)
            {
                throw new ArgumentNullException(nameof(hold));
            }

            if (problemDbo is null)
            {
                throw new ArgumentNullException(nameof(problemDbo));
            }

            var problemHold = new ProblemHold()
            {
                HoldId = hold.HoldId,
                Problem = problemDbo,
                Position = index,
                IsStandingStartHold = hold.IsStandingStartHold
            };

            problemHold.ProblemHoldRules = CreateProblemHoldRuleDbos(hold, problemHold).ToList();

            return problemHold;
        }

        private static IEnumerable<ProblemHoldRule> CreateProblemHoldRuleDbos(CreateHoldOnProblemRequest hold, ProblemHold problemHold)
        {
            if (hold is null)
            {
                throw new ArgumentNullException(nameof(hold));
            }

            if (problemHold is null)
            {
                throw new ArgumentNullException(nameof(problemHold));
            }

            var problemHoldRules = new List<ProblemHoldRule>();

            if (hold.ExistingHoldRuleIds != null)
            {
                problemHoldRules.AddRange(
                    hold.ExistingHoldRuleIds
                    .Select(ruleId => new ProblemHoldRule()
                    {
                        ProblemHold = problemHold,
                        HoldRuleId = ruleId
                    }));
            }

            if (hold.NewHoldRules != null)
            {
                problemHoldRules.AddRange(
                    hold.NewHoldRules
                    .Select(rule => new ProblemHoldRule()
                    {
                        ProblemHold = problemHold,
                        HoldRule = new HoldRule()
                        {
                            Name = rule.Name,
                            Description = rule.Description
                        }
                    }));
            }

            return problemHoldRules;
        }
    }
}
