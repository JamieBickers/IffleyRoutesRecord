using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace IffleyRoutesRecord.IntegrationTests
{
    public class TestRunner
    {
        private readonly ClientApi clientApi;

        public TestRunner(ClientApi clientApi)
        {
            this.clientApi = clientApi;
        }

        public async Task GetAndAssertResultEqualsExpectedAsync<T>(Uri uri, T expectedResult)
        {
            var result = await clientApi.GetAsync<T>(uri);
            AssertAreEqual(result, expectedResult);
        }

        public async Task GetAndAssertResultSatisfiesPredicateAsync<T>(Uri uri, Predicate<T> predicate)
        {
            var result = await clientApi.GetAsync<T>(uri);
            AssertIsSatisfied(result, predicate);
        }

        public async Task PostAndAssertResultEqualsAsync<TContent, TResponse>(Uri uri, TContent content, TResponse expectedResult)
        {
            var result = await clientApi.PostAsync<TContent, TResponse>(uri, content);
            AssertAreEqual(result, expectedResult);
        }

        private static void AssertAreEqual<T>(T first, T second)
        {
            string firstSerialized = Serialize(first);
            string secondSerialized = Serialize(second);

            if (!firstSerialized.Equals(secondSerialized, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new NotEqualException();
            }
        }

        private static void AssertIsSatisfied<T>(T result, Predicate<T> predicate)
        {
            if (!predicate(result))
            {
                throw new ConditionNotSatisfiedException();
            }
        }

        private static string Serialize(object data)
        {
            return JsonConvert.SerializeObject(data).ToUpperInvariant();
        }
    }
}
