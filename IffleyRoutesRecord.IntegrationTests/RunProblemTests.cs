using IffleyRoutesRecord.Logic.DTOs.Received;
using IffleyRoutesRecord.Logic.DTOs.Sent;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IffleyRoutesRecord.IntegrationTests
{
    static class RunProblemTests
    {
        public static async Task Run(Uri baseUri, HttpClient httpClient)
        {
            var problemUri = new Uri(baseUri, "problem/");

            var problemToCreate1 = ProblemToCreate1;
            var problemToCreate2 = ProblemToCreate2;

            string asJsonString1 = JsonConvert.SerializeObject(problemToCreate1);
            string asJsonString2 = JsonConvert.SerializeObject(problemToCreate2);

            var putResult1 = await httpClient.PutAsync(problemUri, new StringContent(asJsonString1, Encoding.UTF8, "application/json"));
            var putResult2 = await httpClient.PutAsync(problemUri, new StringContent(asJsonString2, Encoding.UTF8, "application/json"));

            string expectedGetResult1 = JsonConvert.SerializeObject(ProblemToRead1).ToLower();
            string expectedGetResult2 = JsonConvert.SerializeObject(ProblemToRead2).ToLower();

            if (!putResult1.IsSuccessStatusCode || !putResult2.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            string putResultContent1 = (await putResult1.Content.ReadAsStringAsync()).ToLower();
            string putResultContent2 = (await putResult2.Content.ReadAsStringAsync()).ToLower();
            if (putResultContent1 != expectedGetResult1 || putResultContent2 != expectedGetResult2)
            {
                throw new Exception();
            }

            var problem1Uri = new Uri(problemUri, "1");
            var problem2Uri = new Uri(problemUri, "2");

            var getResult1 = await httpClient.GetAsync(problem1Uri);
            var getResult2 = await httpClient.GetAsync(problem2Uri);

            string getResultContent1 = (await getResult1.Content.ReadAsStringAsync()).ToLower();
            string getResultContent2 = (await getResult2.Content.ReadAsStringAsync()).ToLower();

            if (expectedGetResult1 != getResultContent1 || expectedGetResult2 != getResultContent2)
            {
                throw new Exception();
            }

            string getResults = (await (await httpClient.GetAsync(problemUri)).Content.ReadAsStringAsync()).ToLower();
            string expectedResults = JsonConvert.SerializeObject(ProblemsToRead).ToLower();

            if (getResults != expectedResults)
            {
                throw new Exception();
            }
        }

        private static readonly ProblemDto ProblemToRead1 = new ProblemDto()
        {
            ProblemId = 1,
            Name = "Test Problem 1",
            Description = "Test Description 1",
            DateSet = DateTimeOffset.Parse("2018-01-01"),
            FirstAscent = "Test First Ascent",
            TechGrade = new TechGradeDto()
            {
                GradeId = 1,
                Name = "4a-",
                Rank = 1
            },
            BGrade = new BGradeDto()
            {
                GradeId = 1,
                Name = "B0",
                Rank = 1
            },
            PoveyGrade = new PoveyGradeDto()
            {
                GradeId = 1,
                Name = "Easy",
                Rank = 1
            },
            FurlongGrade = new FurlongGradeDto()
            {
                GradeId = 1,
                Name = "A",
                Rank = 1
            },
            Holds = new List<HoldOnProblemDto>()
            {
                new HoldOnProblemDto()
                {
                    HoldId = 46,
                    Name = "46",
                    IsStandingStartHold = true,
                    HoldRules = new List<HoldRuleDto>()
                },
                new HoldOnProblemDto()
                {
                    HoldId = 45,
                    Name = "45",
                    IsStandingStartHold = true,
                    HoldRules = new List<HoldRuleDto>()
                    {
                        new HoldRuleDto()
                        {
                            HoldRuleId = 1,
                            Name = "Slope Only"
                        }
                    }
                }
            },
            Rules = new List<ProblemRuleDto>()
            {
                new ProblemRuleDto()
                {
                    ProblemRuleId = 1,
                    Name = "No Chips"
                }
            },
            StyleSymbols = new List<StyleSymbolDto>()
            {
                new StyleSymbolDto()
                {
                    StyleSymbolId = 1,
                    Name = "One Star",
                    Description = ""
                },
                new StyleSymbolDto()
                {
                    StyleSymbolId = 2,
                    Name = "Two Stars",
                    Description = ""
                },
                new StyleSymbolDto()
                {
                    StyleSymbolId = 3,
                    Name = "Three Stars",
                    Description = ""
                }
            }
        };

        private static readonly ProblemDto ProblemToRead2 = new ProblemDto()
        {
            ProblemId = 2,
            Name = "Test Problem 2",
            Description = "Test Description 2",
            FirstAscent = "Test First Ascent",
            TechGrade = new TechGradeDto()
            {
                GradeId = 1,
                Name = "4a-",
                Rank = 1
            },
            Holds = new List<HoldOnProblemDto>()
            {
                new HoldOnProblemDto()
                {
                    HoldId = 85,
                    Name = "85",
                    IsStandingStartHold = true,
                    HoldRules = new List<HoldRuleDto>()
                },
                new HoldOnProblemDto()
                {
                    HoldId = 55,
                    Name = "55",
                    IsStandingStartHold = true,
                    HoldRules = new List<HoldRuleDto>()
                    {
                        new HoldRuleDto()
                        {
                            HoldRuleId = 1,
                            Name = "Slope Only"
                        }
                    }
                }
            },
            Rules = new List<ProblemRuleDto>(),
            StyleSymbols = new List<StyleSymbolDto>()
        };

        private static readonly List<ProblemDto> ProblemsToRead = new List<ProblemDto>()
        {
            ProblemToRead1,
            ProblemToRead2
        };

        private static CreateProblemDto ProblemToCreate1 = new CreateProblemDto()
        {
            Name = "Test Problem 1",
            Description = "Test Description 1",
            DateSet = DateTimeOffset.Parse("2018-01-01"),
            FirstAscent = "Test First Ascent",
            TechGradeId = 1,
            BGradeId = 1,
            PoveyGradeId = 1,
            FurlongGradeId = 1,
            Holds = new List<CreateHoldOnProblemDto>()
                    {
                        new CreateHoldOnProblemDto()
                        {
                            HoldId = 46,
                            IsStandingStartHold = true
                        },
                        new CreateHoldOnProblemDto()
                        {
                            HoldId = 45,
                            IsStandingStartHold = true,
                            NewHoldRules = new List<CreateHoldRuleDto>()
                            {
                                new CreateHoldRuleDto()
                                {
                                    Name = "Slope Only"
                                }
                            }
                        }
                    },
            NewRules = new List<CreateProblemRuleDto>()
                    {
                        new CreateProblemRuleDto()
                        {
                            Name = "No Chips"
                        }
                    },
            StyleSymbolIds = new List<int>() { 1, 2, 3 }
        };

        private static readonly CreateProblemDto ProblemToCreate2 = new CreateProblemDto()
        {
            Name = "Test Problem 2",
            Description = "Test Description 2",
            DateSet = null,
            FirstAscent = "Test First Ascent",
            TechGradeId = 1,
            Holds = new List<CreateHoldOnProblemDto>()
            {
                new CreateHoldOnProblemDto()
                {
                    HoldId = 85,
                    IsStandingStartHold = true
                },
                new CreateHoldOnProblemDto()
                {
                    HoldId = 55,
                    IsStandingStartHold = true,
                    ExistingHoldRuleIds = new List<int>() { 1 }
                }
            }
        };
    }
}
