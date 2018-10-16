using IffleyRoutesRecord.IntegrationTests.ApiTests;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace IffleyRoutesRecord.IntegrationTests
{
    internal static class Program
    {
        static async Task Main()
        {
            var baseUri = new Uri("http://localhost:61469/api/");
            var clientApi = new ClientApi(HttpClientFactory.Create());
            var testRunner = new TestRunner(clientApi);

            await RunProblemTests.Run(baseUri, testRunner);
            await RunSimpleInvalidProblemTests.Run(baseUri, testRunner);
            await RunStyleSymbolTests.Run(baseUri, testRunner);
            await RunHoldTests.Run(baseUri, testRunner);
            await RunRuleTests.Run(baseUri, testRunner);
            await RunGradeTests.Run(baseUri, testRunner);

            await clientApi.ShutdownAsync(baseUri);
        }
    }
}
