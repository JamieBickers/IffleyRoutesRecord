using System;
using System.Net.Http;
using Microsoft.Data.Sqlite;
using System.IO;
using System.Threading.Tasks;

namespace IffleyRoutesRecord.IntegrationTests
{
    class Program
    {
        static async Task Main()
        {
            ResetDatabase();

            var baseUri = new Uri("http://localhost:61469/");
            var httpClient = HttpClientFactory.Create();

            //TODO: Add rule tests
            await Task.WhenAll(
                RunProblemTests.Run(baseUri, httpClient),
                RunStyleSymbolTests.Run(baseUri, httpClient),
                RunHoldTests.Run(baseUri, httpClient));

            await Shutdown(baseUri, httpClient);
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

        private static async Task Shutdown(Uri baseUri, HttpClient httpClient)
        {
            await httpClient.GetAsync(new Uri(baseUri, "test/shutdown"));
        }
    }
}
