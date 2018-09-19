using IffleyRoutesRecord.Models.DTOs.Requests;
using IffleyRoutesRecord.Models.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace IffleyRoutesRecord.IntegrationTests
{
    public static class RunRuleTests
    {
        public static async Task Run(Uri baseUri, TestRunner testRunner)
        {
            var problemUri = new Uri(baseUri, "problem/");
            var holdRuleUri = new Uri(baseUri, "rule/hold/");
            var problemRuleUri = new Uri(baseUri, "rule/problem/");

            await testRunner.PostAndAssertResultEqualsAsync(problemUri, ProblemToCreate, ProblemToRead);

            await testRunner.GetAndAssertResultEqualsExpectedAsync(new Uri(holdRuleUri, "2"), HoldRuleToRead);
            await testRunner.GetAndAssertResultEqualsExpectedAsync(new Uri(problemRuleUri, "2"), ProblemRuleToRead);

            await testRunner.GetAndAssertResultSatisfiesPredicateAsync<IEnumerable<HoldRuleResponse>>(holdRuleUri, result => result.Count() == 2);
            await testRunner.GetAndAssertResultSatisfiesPredicateAsync<IEnumerable<ProblemRuleResponse>>(problemRuleUri, result => result.Count() == 2);
        }

        private static readonly ProblemResponse ProblemToRead = new ProblemResponse()
        {
            ProblemId = 3,
            Name = "Test Problem 1 For Rules",
            Description = "Test Description 1",
            DateSet = DateTimeOffset.Parse("2018-01-01", CultureInfo.InvariantCulture),
            FirstAscent = "Test First Ascent",
            TechGrade = new TechGradeResponse()
            {
                GradeId = 1,
                Name = "4a-",
                Rank = 1
            },
            BGrade = new BGradeResponse()
            {
                GradeId = 1,
                Name = "B0",
                Rank = 1
            },
            PoveyGrade = new PoveyGradeResponse()
            {
                GradeId = 1,
                Name = "Easy",
                Rank = 1
            },
            FurlongGrade = new FurlongGradeResponse()
            {
                GradeId = 1,
                Name = "A",
                Rank = 1
            },
            Holds = new List<HoldOnProblemResponse>()
            {
                new HoldOnProblemResponse()
                {
                    HoldId = 46,
                    Name = "46",
                    IsStandingStartHold = true,
                    HoldRules = new List<HoldRuleResponse>()
                },
                new HoldOnProblemResponse()
                {
                    HoldId = 45,
                    Name = "45",
                    IsStandingStartHold = true,
                    HoldRules = new List<HoldRuleResponse>()
                    {
                        new HoldRuleResponse()
                        {
                            HoldRuleId = 2,
                            Name = "Hands Only"
                        }
                    }
                }
            },
            Rules = new List<ProblemRuleResponse>()
            {
                new ProblemRuleResponse()
                {
                    ProblemRuleId = 2,
                    Name = "Right Foot Only"
                }
            },
            StyleSymbols = new List<StyleSymbolResponse>()
            {
                new StyleSymbolResponse()
                {
                    StyleSymbolId = 1,
                    Name = "One Star",
                    Description = ""
                },
                new StyleSymbolResponse()
                {
                    StyleSymbolId = 2,
                    Name = "Two Stars",
                    Description = ""
                },
                new StyleSymbolResponse()
                {
                    StyleSymbolId = 3,
                    Name = "Three Stars",
                    Description = ""
                }
            }
        };

        private static readonly CreateProblemRequest ProblemToCreate = new CreateProblemRequest()
        {
            Name = "Test Problem 1 For Rules",
            Description = "Test Description 1",
            DateSet = DateTimeOffset.Parse("2018-01-01", CultureInfo.InvariantCulture),
            FirstAscent = "Test First Ascent",
            TechGradeId = 1,
            BGradeId = 1,
            PoveyGradeId = 1,
            FurlongGradeId = 1,
            Holds = new List<CreateHoldOnProblemRequest>()
                    {
                        new CreateHoldOnProblemRequest()
                        {
                            HoldId = 46,
                            IsStandingStartHold = true
                        },
                        new CreateHoldOnProblemRequest()
                        {
                            HoldId = 45,
                            IsStandingStartHold = true,
                            NewHoldRules = new List<CreateHoldRuleRequest>()
                            {
                                new CreateHoldRuleRequest()
                                {
                                    Name = "Hands Only"
                                }
                            }
                        }
                    },
            NewRules = new List<CreateProblemRuleRequest>()
                    {
                        new CreateProblemRuleRequest()
                        {
                            Name = "Right Foot Only"
                        }
                    },
            StyleSymbolIds = new List<int>() { 1, 2, 3 }
        };

        private static readonly HoldRuleResponse HoldRuleToRead = new HoldRuleResponse()
        {
            HoldRuleId = 2,
            Name = "Hands Only"
        };

        private static readonly ProblemRuleResponse ProblemRuleToRead = new ProblemRuleResponse()
        {
            ProblemRuleId = 2,
            Name = "Right Foot Only"
        };
    }
}
