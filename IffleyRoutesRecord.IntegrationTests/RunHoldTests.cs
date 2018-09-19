using IffleyRoutesRecord.Models.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IffleyRoutesRecord.IntegrationTests
{
    public static class RunHoldTests
    {
        public static async Task Run(Uri baseUri, TestRunner testRunner)
        {
            var holdUri = new Uri(baseUri, "hold/");
            var hold1Uri = new Uri(holdUri, "1");

            await testRunner.GetAndAssertResultEqualsExpectedAsync(hold1Uri, hold);

            await testRunner.GetAndAssertResultSatisfiesPredicateAsync<IEnumerable<HoldResponse>>(holdUri,
                result => result.Count() >= 117 && !result.Any(holdResult => holdResult.HoldId == 0 || string.IsNullOrWhiteSpace(holdResult.Name)));
        }

        private static readonly HoldResponse hold = new HoldResponse()
        {
            HoldId = 1,
            Name = "1"
        };
    }
}
