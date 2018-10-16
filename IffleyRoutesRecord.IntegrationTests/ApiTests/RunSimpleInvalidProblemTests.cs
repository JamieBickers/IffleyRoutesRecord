using IffleyRoutesRecord.Models.DTOs.Requests;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IffleyRoutesRecord.IntegrationTests.ApiTests
{
    static class RunSimpleInvalidProblemTests
    {
        public static async Task Run(Uri baseUri, TestRunner testRunner)
        {
            var problemUri = new Uri(baseUri, "problem/");

            await testRunner.PostWithModelErrorsAsync(problemUri, InvalidProblemWithoutRequiredData, InvalidProblemWithoutRequiredDataError);
            await testRunner.PostWithModelErrorsAsync(problemUri, InvalidProblemWithDataOutOfRange, InvalidProblemWithDataOutOfRangeError);
            await testRunner.PostWithModelErrorsAsync(problemUri, InvalidHoldWithDataOutOfRange, InvalidHoldWithDataOutOfRangeError);
            await testRunner.PostWithModelErrorsAsync(problemUri, InvalidHoldRuleWithoutRequiredData, InvalidHoldRuleWithoutRequiredDataError);
            await testRunner.PostWithModelErrorsAsync(problemUri, InvalidRuleWithoutRequiredData, InvalidRuleWithoutRequiredDataError);
        }

        private static readonly CreateProblemRequest InvalidProblemWithoutRequiredData = new CreateProblemRequest()
        {
            TechGradeId = 1,
            Holds = new List<CreateHoldOnProblemRequest>()
            {
                new CreateHoldOnProblemRequest()
                {
                    HoldId = 1
                }
            }
        };

        private const string InvalidProblemWithoutRequiredDataError =
            "{\"Name\":[\"The Name field is required.\"]}";

        private static readonly CreateProblemRequest InvalidProblemWithDataOutOfRange = new CreateProblemRequest()
        {
            Name = new string('a', 101),
            Description = new string ('a', 5001),
            FirstAscent = new string ('a', 101),
            TechGradeId = 0,
            BGradeId = 0,
            PoveyGradeId = 0,
            FurlongGradeId = 0
        };

        private const string InvalidProblemWithDataOutOfRangeError =
            "{\"Name\":[\"The field Name must be a string or array type with a maximum length of '100'.\"]"
            + ",\"BGradeId\":[\"The field BGradeId must be between 1 and 2147483647.\"],\"Description\":"
            + "[\"The field Description must be a string or array type with a maximum length of '5000'.\"],\"FirstAscent\":"
            + "[\"The field FirstAscent must be a string or array type with a maximum length of '100'.\"],\"TechGradeId\":"
            + "[\"The field TechGradeId must be between 1 and 2147483647.\"],\"PoveyGradeId\":[\"The field PoveyGradeId"
            + " must be between 1 and 2147483647.\"],\"FurlongGradeId\":[\"The field FurlongGradeId must be between 1 and 2147483647.\"]}";

        private static readonly CreateProblemRequest InvalidHoldWithDataOutOfRange = new CreateProblemRequest()
        {
            Name = "a",
            TechGradeId = 1,
            Holds = new List<CreateHoldOnProblemRequest>()
            {
                new CreateHoldOnProblemRequest()
                {

                }
            }
        };

        private const string InvalidHoldWithDataOutOfRangeError =
            "{\"Holds[0].HoldId\":[\"The field HoldId must be between 1 and 2147483647.\"]}";

        private static readonly CreateProblemRequest InvalidHoldRuleWithoutRequiredData = new CreateProblemRequest()
        {
            Name = "a",
            TechGradeId = 1,
            Holds = new List<CreateHoldOnProblemRequest>()
            {
                new CreateHoldOnProblemRequest()
                {
                    HoldId = 1,
                    NewHoldRules = new List<CreateHoldRuleRequest>()
                    {
                        new CreateHoldRuleRequest()
                    }
                }
            }
        };

        private const string InvalidHoldRuleWithoutRequiredDataError =
            "{\"Holds[0].NewHoldRules[0].Name\":[\"The Name field is required.\"]}";

        private static readonly CreateProblemRequest InvalidRuleWithoutRequiredData = new CreateProblemRequest()
        {
            Name = "a",
            TechGradeId = 1,
            Holds = new List<CreateHoldOnProblemRequest>()
            {
                new CreateHoldOnProblemRequest()
                {
                    HoldId = 1
                }
            }, NewRules = new List<CreateProblemRuleRequest>()
            {
                new CreateProblemRuleRequest()
            }
        };

        private const string InvalidRuleWithoutRequiredDataError =
            "{\"NewRules[0].Name\":[\"The Name field is required.\"]}";
    }
}
