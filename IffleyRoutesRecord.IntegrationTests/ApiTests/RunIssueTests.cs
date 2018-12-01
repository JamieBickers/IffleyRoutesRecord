using IffleyRoutesRecord.Models.DTOs.Requests;
using IffleyRoutesRecord.Models.DTOs.Responses;
using IffleyRoutesRecord.Models.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace IffleyRoutesRecord.IntegrationTests.ApiTests
{
    static class RunIssueTests
    {
        public static async Task Run(Uri baseUri, TestRunner testRunner)
        {
            var issueUri = new Uri(baseUri, "issue/");
            var problemIssueUri = new Uri(issueUri, "problem");

            await testRunner.PostAndAssertIsSuccessfulAsync(issueUri, IssueToCreate1);
            await testRunner.PostAndAssertIsSuccessfulAsync(issueUri, IssueToCreate2);
            await testRunner.GetAndAssertResultEqualsExpectedAsync(issueUri, IssuesToRead);

            await testRunner.PostAndAssertIsSuccessfulAsync(problemIssueUri, ProblemIssueToCreate1);
            await testRunner.PostAndAssertIsSuccessfulAsync(problemIssueUri, ProblemIssueToCreate2);
            await testRunner.GetAndAssertResultEqualsExpectedAsync(problemIssueUri, ProblemIssuesToRead);
        }

        private static readonly CreateIssueRequest IssueToCreate1 = new CreateIssueRequest()
        {
            Description = "issue 1",
            SubmittedBy = "Jamie Bickers"
        };

        private static readonly CreateIssueRequest IssueToCreate2 = new CreateIssueRequest()
        {
            Description = "issue 2",
            SubmittedBy = "Jamie M A Bickers"
        };

        private static readonly List<Issue> IssuesToRead = new List<Issue>()
        {
            new Issue()
            {
                Id = 1,
                Description = "issue 1",
                SubmittedBy = "Jamie Bickers"
            },
            new Issue()
            {
                Id = 2,
                Description = "issue 2",
                SubmittedBy = "Jamie M A Bickers"
            }
        };

        private static readonly CreateProblemIssueRequest ProblemIssueToCreate1 = new CreateProblemIssueRequest()
        {
            ProblemId = 1,
            Description = "problem issue 1",
            SubmittedBy = "Jamie Bickers"
        };

        private static readonly CreateProblemIssueRequest ProblemIssueToCreate2 = new CreateProblemIssueRequest()
        {
            ProblemId = 2,
            Description = "problem issue 2",
            SubmittedBy = "Jamie M A Bickers"
        };

        private static readonly List<ProblemIssueResponse> ProblemIssuesToRead = new List<ProblemIssueResponse>()
        {
            new ProblemIssueResponse()
            {
                ProblemIssueId = 1,
                Description = "problem issue 1",
                SubmittedBy = "Jamie Bickers",
                Problem = new ProblemResponse()
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
                    GlobalGrade = 1,
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
                }
            },
            new ProblemIssueResponse()
            {
                ProblemIssueId = 2,
                Description = "problem issue 2",
                SubmittedBy = "Jamie M A Bickers",
                Problem = new ProblemResponse()
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
                    GlobalGrade = 1,
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
                }
            }
        };
    }
}