using IffleyRoutesRecord.Logic.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IffleyRoutesRecord.IntegrationTests
{
    public static class RunStyleSymbolTests
    {
        public static async Task Run(Uri baseUri, TestRunner testRunner)
        {
            var styleSymbolUri = new Uri(baseUri, "styleSymbol/");

            await testRunner.GetAndAssertResultEqualsExpectedAsync(new Uri(styleSymbolUri, "1"), StyleSymbol);
            await testRunner.GetAndAssertResultEqualsExpectedAsync(styleSymbolUri, StyleSymbols);
        }

        private static readonly StyleSymbolResponse StyleSymbol = new StyleSymbolResponse()
        {
            StyleSymbolId = 1,
            Name = "One Star",
            Description = ""
        };

        private static readonly List<StyleSymbolResponse> StyleSymbols = new List<StyleSymbolResponse>()
        {
            new StyleSymbolResponse()
            {
                StyleSymbolId = 1,
                Name = "One Star",
                Description = ""
            },
            new StyleSymbolResponse()
            {
                StyleSymbolId = 2,
                Name = "Two Stars",
                Description = ""
            },
            new StyleSymbolResponse()
            {
                StyleSymbolId = 3,
                Name = "Three Stars",
                Description = ""
            },
            new StyleSymbolResponse()
            {
                StyleSymbolId = 4,
                Name = "Four Stars",
                Description = ""
            },
            new StyleSymbolResponse()
            {
                StyleSymbolId = 5,
                Name = "Suitable for All",
                Description = ""
            },
            new StyleSymbolResponse()
            {
                StyleSymbolId = 6,
                Name = "Tall Man",
                Description = ""
            },
            new StyleSymbolResponse()
            {
                StyleSymbolId = 7,
                Name = "Technical",
                Description = ""
            },
            new StyleSymbolResponse()
            {
                StyleSymbolId = 8,
                Name = "Flexible",
                Description = ""
            },
            new StyleSymbolResponse()
            {
                StyleSymbolId = 9,
                Name = "Strong Man",
                Description = ""
            },
            new StyleSymbolResponse()
            {
                StyleSymbolId = 10,
                Name = "Dynamic",
                Description = ""
            },
            new StyleSymbolResponse()
            {
                StyleSymbolId = 11,
                Name = "Fingery",
                Description = ""
            },
            new StyleSymbolResponse()
            {
                StyleSymbolId = 12,
                Name = "Infamous",
                Description = ""
            },
            new StyleSymbolResponse()
            {
                StyleSymbolId = 13,
                Name = "Ambulance",
                Description = ""
            },
        };
    }
}