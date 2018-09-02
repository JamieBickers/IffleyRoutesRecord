using IffleyRoutesRecord.Logic.DTOs.Sent;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IffleyRoutesRecord.IntegrationTests
{
    public static class RunStyleSymbolTests
    {
        public static async Task Run(Uri baseUri, HttpClient httpClient)
        {
            var styleSymbolUri = new Uri(baseUri, "styleSymbol/");

            string getResult = (await (await httpClient.GetAsync(new Uri(styleSymbolUri, "1"))).Content.ReadAsStringAsync()).ToLower();
            string expectedGetResult = JsonConvert.SerializeObject(StyleSymbol).ToLower();

            if (getResult != expectedGetResult)
            {
                throw new Exception();
            }

            string getResults = (await (await httpClient.GetAsync(styleSymbolUri)).Content.ReadAsStringAsync()).ToLower();
            string expectedGetResults = JsonConvert.SerializeObject(StyleSymbols).ToLower();

            if (getResults != expectedGetResults)
            {
                throw new Exception();
            }
        }

        private static readonly StyleSymbolDto StyleSymbol = new StyleSymbolDto()
        {
            StyleSymbolId = 1,
            Name = "One Star",
            Description = ""
        };

        private static readonly List<StyleSymbolDto> StyleSymbols = new List<StyleSymbolDto>()
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
            },
            new StyleSymbolDto()
            {
                StyleSymbolId = 4,
                Name = "Four Stars",
                Description = ""
            },
            new StyleSymbolDto()
            {
                StyleSymbolId = 5,
                Name = "Suitable for All",
                Description = ""
            },
            new StyleSymbolDto()
            {
                StyleSymbolId = 6,
                Name = "Tall Man",
                Description = ""
            },
            new StyleSymbolDto()
            {
                StyleSymbolId = 7,
                Name = "Technical",
                Description = ""
            },
            new StyleSymbolDto()
            {
                StyleSymbolId = 8,
                Name = "Flexible",
                Description = ""
            },
            new StyleSymbolDto()
            {
                StyleSymbolId = 9,
                Name = "Strong Man",
                Description = ""
            },
            new StyleSymbolDto()
            {
                StyleSymbolId = 10,
                Name = "Dynamic",
                Description = ""
            },
            new StyleSymbolDto()
            {
                StyleSymbolId = 11,
                Name = "Fingery",
                Description = ""
            },
            new StyleSymbolDto()
            {
                StyleSymbolId = 12,
                Name = "Infamous",
                Description = ""
            },
            new StyleSymbolDto()
            {
                StyleSymbolId = 13,
                Name = "Ambulance",
                Description = ""
            },
        };
    }
}