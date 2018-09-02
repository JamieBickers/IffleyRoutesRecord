using IffleyRoutesRecord.Logic.DTOs.Sent;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IffleyRoutesRecord.IntegrationTests
{
    public static class RunHoldTests
    {
        public static async Task Run(Uri baseUri, HttpClient httpClient)
        {
            var holdUri = new Uri(baseUri, "hold/");

            string getResult = (await (await httpClient.GetAsync(new Uri(holdUri, "1"))).Content.ReadAsStringAsync()).ToLower();
            string expectedGetResult = JsonConvert.SerializeObject(hold).ToLower();

            var getResults = (await (await httpClient.GetAsync(holdUri)).Content.ReadAsAsync<IEnumerable<HoldDto>>());

            if (getResults.Count() < 117)
            {
                throw new Exception();
            }

            if (getResults.Any(holdResult => holdResult.HoldId == 0 || string.IsNullOrWhiteSpace(holdResult.Name)))
            {
                throw new Exception();
            }
        }

        private static readonly HoldDto hold = new HoldDto()
        {
            HoldId = 1,
            Name = "1"
        };
    }
}
