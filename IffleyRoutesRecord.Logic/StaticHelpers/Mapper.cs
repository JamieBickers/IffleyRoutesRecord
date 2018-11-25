using IffleyRoutesRecord.Models.DTOs.Requests;
using IffleyRoutesRecord.Models.DTOs.Responses;
using IffleyRoutesRecord.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IffleyRoutesRecord.Logic.StaticHelpers
{
    /// <summary>
    /// Maps between database entities and DTOs
    /// </summary>
    internal static class Mapper
    {
        /// <summary>
        /// Create a Problem entity from a CreateProblemRequest DTO
        /// </summary>
        /// <param name="problem">The DTO to map from</param>
        /// <returns>The corresponding entity</returns>
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

        /// <summary>
        /// Create a ProblemResponse DTO entity from a Problem entity
        /// </summary>
        /// <param name="problem">The entity to map from</param>
        /// <returns>The corresponding DTO</returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static ProblemResponse Map(Problem problem)
        {
            if (problem is null)
            {
                throw new ArgumentNullException(nameof(problem));
            }

            return new ProblemResponse()
            {
                ProblemId = problem.Id,
                Name = problem.Name,
                Description = problem.Description,
                DateSet = problem.DateSet,
                FirstAscent = problem.FirstAscent,
                TechGrade = problem.TechGradeId is null ? null : Map(problem.TechGrade),
                BGrade = problem.BGradeId is null ? null : Map(problem.BGrade),
                PoveyGrade = problem.PoveyGradeId is null ? null : Map(problem.PoveyGrade),
                FurlongGrade = problem.FurlongGradeId is null ? null : Map(problem.FurlongGrade),
                Holds = problem.ProblemHolds
                .OrderBy(problemHold => problemHold.Position)
                .Select(problemHold => new HoldOnProblemResponse()
                {
                    HoldId = problemHold.Hold.Id,
                    Name = problemHold.Hold.Name,
                    IsStandingStartHold = problemHold.IsStandingStartHold,
                    HoldRules = problemHold.ProblemHoldRules.Select(problemHoldRule => new HoldRuleResponse()
                    {
                        HoldRuleId = problemHoldRule.HoldRule.Id,
                        Name = problemHoldRule.HoldRule.Name,
                        Description = problemHoldRule.HoldRule.Description
                    })
                })
                .ToList(),
                Rules = problem.ProblemRules.Select(Map),
                StyleSymbols = problem.ProblemStyleSymbols.Select(problemStyleSymbol => new StyleSymbolResponse()
                {
                    StyleSymbolId = problemStyleSymbol.StyleSymbolId,
                    Name = problemStyleSymbol.StyleSymbol.Name,
                    Description = problemStyleSymbol.StyleSymbol.Description
                })
            };
        }

        /// <summary>
        /// Create a TechGradeResponse DTO entity from a TechGrade entity
        /// </summary>
        /// <param name="grade">The entity to map from</param>
        /// <returns>The corresponding DTO</returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static TechGradeResponse Map(TechGrade grade)
        {
            if (grade is null)
            {
                throw new ArgumentNullException(nameof(grade));
            }

            return new TechGradeResponse()
            {
                GradeId = grade.Id,
                Name = grade.Name,
                Rank = grade.Rank
            };
        }

        /// <summary>
        /// Create a BGradeResponse DTO entity from a BGrade entity
        /// </summary>
        /// <param name="grade">The entity to map from</param>
        /// <returns>The corresponding DTO</returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static BGradeResponse Map(BGrade grade)
        {
            if (grade is null)
            {
                throw new ArgumentNullException(nameof(grade));
            }

            return new BGradeResponse()
            {
                GradeId = grade.Id,
                Name = grade.Name,
                Rank = grade.Rank
            };
        }

        /// <summary>
        /// Create a PoveyGradeResponse DTO entity from a PoveyGrade entity
        /// </summary>
        /// <param name="grade">The entity to map from</param>
        /// <returns>The corresponding DTO</returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static PoveyGradeResponse Map(PoveyGrade grade)
        {
            if (grade is null)
            {
                throw new ArgumentNullException(nameof(grade));
            }

            return new PoveyGradeResponse()
            {
                GradeId = grade.Id,
                Name = grade.Name,
                Rank = grade.Rank
            };
        }

        /// <summary>
        /// Create a FurlongGradeResponse DTO entity from a FurlongGrade entity
        /// </summary>
        /// <param name="grade">The entity to map from</param>
        /// <returns>The corresponding DTO</returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static FurlongGradeResponse Map(FurlongGrade grade)
        {
            if (grade is null)
            {
                throw new ArgumentNullException(nameof(grade));
            }

            return new FurlongGradeResponse()
            {
                GradeId = grade.Id,
                Name = grade.Name,
                Rank = grade.Rank
            };
        }

        /// <summary>
        /// Create a HoldResponse DTO entity from a Hold entity
        /// </summary>
        /// <param name="hold">The entity to map from</param>
        /// <returns>The corresponding DTO</returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static HoldResponse Map(Hold hold)
        {
            if (hold is null)
            {
                throw new ArgumentNullException(nameof(hold));
            }

            return new HoldResponse()
            {
                HoldId = hold.Id,
                Name = hold.Name,
            };
        }

        /// <summary>
        /// Create a ProblemRuleResponse DTO entity from a GeneralRule entity
        /// </summary>
        /// <param name="rule">The entity to map from</param>
        /// <returns>The corresponding DTO</returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static ProblemRuleResponse Map(GeneralRule rule)
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

        /// <summary>
        /// Create a HoldRuleResponse DTO entity from a HoldRule entity
        /// </summary>
        /// <param name="rule">The entity to map from</param>
        /// <returns>The corresponding DTO</returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static HoldRuleResponse Map(HoldRule rule)
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

        /// <summary>
        /// Create a ProblemRuleResponse DTO entity from a ProblemRule entity
        /// </summary>
        /// <param name="rule">The entity to map from</param>
        /// <returns>The corresponding DTO</returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static ProblemRuleResponse Map(ProblemRule rule)
        {
            if (rule is null)
            {
                throw new ArgumentNullException(nameof(rule));
            }

            return new ProblemRuleResponse()
            {
                ProblemRuleId = rule.GeneralRuleId,
                Name = rule.GeneralRule.Name,
                Description = rule.GeneralRule.Description
            };
        }

        /// <summary>
        /// Create a StyleSymbolResponse DTO entity from a StyleSymbol entity
        /// </summary>
        /// <param name="styleSymbol">The entity to map from</param>
        /// <returns>The corresponding DTO</returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static StyleSymbolResponse Map(StyleSymbol styleSymbol)
        {
            if (styleSymbol is null)
            {
                throw new ArgumentNullException(nameof(styleSymbol));
            }

            return new StyleSymbolResponse()
            {
                StyleSymbolId = styleSymbol.Id,
                Name = styleSymbol.Name,
                Description = styleSymbol.Description
            };
        }

        /// <summary>
        /// Create a StyleSymbolResponse DTO entity from a ProblemStyleSymbol entity
        /// </summary>
        /// <param name="problemStyleSymbol">The entity to map from</param>
        /// <returns>The corresponding DTO</returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static StyleSymbolResponse Map(ProblemStyleSymbol problemStyleSymbol)
        {
            if (problemStyleSymbol is null)
            {
                throw new ArgumentNullException(nameof(problemStyleSymbol));
            }

            return new StyleSymbolResponse()
            {
                StyleSymbolId = problemStyleSymbol.StyleSymbolId,
                Name = problemStyleSymbol.StyleSymbol.Name,
                Description = problemStyleSymbol.StyleSymbol.Description
            };
        }

        /// <summary>
        /// Create an Issue entity from a CreateIssueRequest DTO
        /// </summary>
        /// <param name="issueRequest">The DTO to map from</param>
        /// <returns>The corresponding entity</returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static Issue Map(CreateIssueRequest issueRequest)
        {
            if (issueRequest is null)
            {
                throw new ArgumentNullException(nameof(issueRequest));
            }

            return new Issue()
            {
                Description = issueRequest.Description,
                SubmittedBy = issueRequest.SubmittedBy
            };
        }

        /// <summary>
        /// Create a ProblemIssue entity from a CreateProblemIssueRequest DTO
        /// </summary>
        /// <param name="issueRequest">The DTO to map from</param>
        /// <returns>The corresponding entity</returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static ProblemIssue Map(CreateProblemIssueRequest issueRequest)
        {
            if (issueRequest is null)
            {
                throw new ArgumentNullException(nameof(issueRequest));
            }

            return new ProblemIssue()
            {
                ProblemId = issueRequest.ProblemId,
                Description = issueRequest.Description,
                SubmittedBy = issueRequest.SubmittedBy
            };
        }

        /// <summary>
        /// Create a ProblemIssueResponse DTO from a ProblemIssue entity
        /// </summary>
        /// <param name="issue">The entity to map from</param>
        /// <returns>The corresponding DTO</returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static ProblemIssueResponse Map(ProblemIssue issue)
        {
            if (issue is null)
            {
                throw new ArgumentNullException(nameof(issue));
            }

            return new ProblemIssueResponse()
            {
                ProblemIssueId = issue.Id,
                Description = issue.Description,
                SubmittedBy = issue.SubmittedBy
            };
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
