using IffleyRoutesRecord.Models.DTOs.Requests;
using IffleyRoutesRecord.Models.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace IffleyRoutesRecord.IntegrationTests.ApiTests
{
    static class RunProblemTests
    {
        public static async Task Run(Uri baseUri, TestRunner testRunner)
        {
            var problemUri = new Uri(baseUri, "problem/");
            var problem1Uri = new Uri(problemUri, "1");
            var problem2Uri = new Uri(problemUri, "2");

            await testRunner.PostAndAssertResultEqualsAsync(problemUri, ProblemToCreate1, ProblemToRead1);
            await testRunner.PostAndAssertResultEqualsAsync(problemUri, ProblemToCreate2, ProblemToRead2);

            // Do in this order with repition to test with and without caching
            await testRunner.GetAndAssertResultEqualsExpectedAsync(problem1Uri, ProblemToRead1);
            await testRunner.GetAndAssertResultEqualsExpectedAsync(problemUri, ProblemsToRead);
            await testRunner.GetAndAssertResultEqualsExpectedAsync(problem2Uri, ProblemToRead2);
            await testRunner.GetAndAssertResultEqualsExpectedAsync(problemUri, ProblemsToRead);
            await testRunner.GetAndAssertResultEqualsExpectedAsync(problem1Uri, ProblemToRead1);
            await testRunner.GetAndAssertResultEqualsExpectedAsync(problem2Uri, ProblemToRead2);
        }

        private static readonly ProblemResponse ProblemToRead1 = new ProblemResponse()
        {
            ProblemId = 1,
            Name = "Test Problem 1",
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
                            HoldRuleId = 1,
                            Name = "Slope Only"
                        }
                    }
                }
            },
            Rules = new List<ProblemRuleResponse>()
            {
                new ProblemRuleResponse()
                {
                    ProblemRuleId = 1,
                    Name = "No Chips"
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

        private static readonly ProblemResponse ProblemToRead2 = new ProblemResponse()
        {
            ProblemId = 2,
            Name = "Test Problem 2",
            Description = "Test Description 2",
            FirstAscent = "Test First Ascent",
            TechGrade = new TechGradeResponse()
            {
                GradeId = 1,
                Name = "4a-",
                Rank = 1
            },
            Holds = new List<HoldOnProblemResponse>()
            {
                new HoldOnProblemResponse()
                {
                    HoldId = 85,
                    Name = "85",
                    IsStandingStartHold = true,
                    HoldRules = new List<HoldRuleResponse>()
                },
                new HoldOnProblemResponse()
                {
                    HoldId = 55,
                    Name = "55",
                    IsStandingStartHold = true,
                    HoldRules = new List<HoldRuleResponse>()
                    {
                        new HoldRuleResponse()
                        {
                            HoldRuleId = 1,
                            Name = "Slope Only"
                        }
                    }
                }
            },
            Rules = new List<ProblemRuleResponse>(),
            StyleSymbols = new List<StyleSymbolResponse>()
        };

        private static readonly List<ProblemResponse> ProblemsToRead = new List<ProblemResponse>()
        {
            ProblemToRead1,
            ProblemToRead2
        };

        private static readonly CreateProblemRequest ProblemToCreate1 = new CreateProblemRequest()
        {
            Name = "Test Problem 1",
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
                                    Name = "Slope Only"
                                }
                            }
                        }
                    },
            NewRules = new List<CreateProblemRuleRequest>()
                    {
                        new CreateProblemRuleRequest()
                        {
                            Name = "No Chips"
                        }
                    },
            StyleSymbolIds = new List<int>() { 1, 2, 3 }
        };

        private static readonly CreateProblemRequest ProblemToCreate2 = new CreateProblemRequest()
        {
            Name = "Test Problem 2",
            Description = "Test Description 2",
            DateSet = null,
            FirstAscent = "Test First Ascent",
            TechGradeId = 1,
            Holds = new List<CreateHoldOnProblemRequest>()
            {
                new CreateHoldOnProblemRequest()
                {
                    HoldId = 85,
                    IsStandingStartHold = true
                },
                new CreateHoldOnProblemRequest()
                {
                    HoldId = 55,
                    IsStandingStartHold = true,
                    ExistingHoldRuleIds = new List<int>() { 1 }
                }
            }
        };
    }
}
