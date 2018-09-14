using System;
using System.Net.Http;
using Microsoft.Data.Sqlite;
using System.IO;
using System.Threading.Tasks;

namespace IffleyRoutesRecord.IntegrationTests
{
    internal static class Program
    {
        static async Task Main()
        {
            ResetDatabase();

            var baseUri = new Uri("http://localhost:61469/");
            var clientApi = new ClientApi(HttpClientFactory.Create());
            var testRunner = new TestRunner(clientApi);

            await RunProblemTests.Run(baseUri, testRunner);
            await RunStyleSymbolTests.Run(baseUri, testRunner);
            await RunHoldTests.Run(baseUri, testRunner);
            await RunRuleTests.Run(baseUri, testRunner);
            await RunGradeTests.Run(baseUri, testRunner);

            await clientApi.ShutdownAsync(baseUri);
        }

        private static void ResetDatabase()
        {
            var connection = new SqliteConnection(@"Data Source=C:\Users\bicke\OneDrive\Desktop\IffleyRoutesRecord\IffleyRoutes.db;");
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = File.ReadAllText(@"C:\Users\bicke\OneDrive\Desktop\IffleyRoutesRecord\SQL\ResetDatabase.sql");
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}
