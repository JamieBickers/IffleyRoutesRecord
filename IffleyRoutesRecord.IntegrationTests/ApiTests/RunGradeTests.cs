using IffleyRoutesRecord.Models.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IffleyRoutesRecord.IntegrationTests.ApiTests
{
    public static class RunGradeTests
    {
        public static async Task Run(Uri baseUri, TestRunner testRunner)
        {
            var gradeUri = new Uri(baseUri, "grade/");
            var techGradeUri = new Uri(gradeUri, "tech/");
            var bGradeUri = new Uri(gradeUri, "b/");
            var poveyGradeUri = new Uri(gradeUri, "povey/");
            var furlongGradeUri = new Uri(gradeUri, "furlong/");

            // Run twice and in this order to test caching
            await testRunner.GetAndAssertResultEqualsExpectedAsync(new Uri(techGradeUri, "1"), TechGradeToRead);
            await testRunner.GetAndAssertResultEqualsExpectedAsync(new Uri(bGradeUri, "1"), BGradeToRead);

            await testRunner.GetAndAssertResultSatisfiesPredicateAsync<IEnumerable<TechGradeResponse>>(techGradeUri, result => result.Count() >= 27);
            await testRunner.GetAndAssertResultSatisfiesPredicateAsync<IEnumerable<TechGradeResponse>>(bGradeUri, result => result.Count() >= 7);
            await testRunner.GetAndAssertResultSatisfiesPredicateAsync<IEnumerable<TechGradeResponse>>(poveyGradeUri, result => result.Count() == 3);
            await testRunner.GetAndAssertResultSatisfiesPredicateAsync<IEnumerable<TechGradeResponse>>(furlongGradeUri, result => result.Count() == 5);

            await testRunner.GetAndAssertResultEqualsExpectedAsync(new Uri(poveyGradeUri, "1"), PoveyGradeToRead);
            await testRunner.GetAndAssertResultEqualsExpectedAsync(new Uri(furlongGradeUri, "1"), FurlongGradeToRead);

            await testRunner.GetAndAssertResultSatisfiesPredicateAsync<IEnumerable<TechGradeResponse>>(techGradeUri, result => result.Count() >= 27);
            await testRunner.GetAndAssertResultSatisfiesPredicateAsync<IEnumerable<TechGradeResponse>>(bGradeUri, result => result.Count() >= 7);
            await testRunner.GetAndAssertResultSatisfiesPredicateAsync<IEnumerable<TechGradeResponse>>(poveyGradeUri, result => result.Count() == 3);
            await testRunner.GetAndAssertResultSatisfiesPredicateAsync<IEnumerable<TechGradeResponse>>(furlongGradeUri, result => result.Count() == 5);
        }

        private static readonly TechGradeResponse TechGradeToRead = new TechGradeResponse()
        {
            GradeId = 1,
            Name = "4a-",
            Rank = 1
        };

        private static readonly BGradeResponse BGradeToRead = new BGradeResponse()
        {
            GradeId = 1,
            Name = "B0",
            Rank = 1
        };

        private static readonly PoveyGradeResponse PoveyGradeToRead = new PoveyGradeResponse()
        {
            GradeId = 1,
            Name = "easy",
            Rank = 1
        };

        private static readonly FurlongGradeResponse FurlongGradeToRead = new FurlongGradeResponse()
        {
            GradeId = 1,
            Name = "A",
            Rank = 1
        };
    }
}
