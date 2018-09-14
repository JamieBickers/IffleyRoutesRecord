using IffleyRoutesRecord.Logic.DataAccess;
using IffleyRoutesRecord.Logic.DTOs.Responses;
using IffleyRoutesRecord.Logic.Entities;
using IffleyRoutesRecord.Logic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IffleyRoutesRecord.Logic.Managers
{
    public class StyleSymbolManager : IStyleSymbolManager
    {
        private readonly IffleyRoutesRecordContext iffleyRoutesRecordContext;
        private readonly IMemoryCache cache;

        public StyleSymbolManager(IffleyRoutesRecordContext iffleyRoutesRecordContext, IMemoryCache cache)
        {
            this.iffleyRoutesRecordContext = iffleyRoutesRecordContext;
            this.cache = cache;
        }

        public StyleSymbolResponse GetStyleSymbol(int styleSymbolId)
        {
            if (cache.TryRetrieveItemWithId<StyleSymbolResponse>(
                styleSymbolId, cachedStyleSymbol => cachedStyleSymbol.StyleSymbolId, out var styleSymbolResponse))
            {
                return styleSymbolResponse;
            }

            var styleSymbol = iffleyRoutesRecordContext.StyleSymbol.Single(symbol => symbol.Id == styleSymbolId);
            return CreateStyleSymbolResponse(styleSymbol);
        }

        public IEnumerable<StyleSymbolResponse> GetStyleSymbols()
        {
            if (cache.TryRetrieveAllItems<StyleSymbolResponse>(out var styleSymbolsFromCache))
            {
                return styleSymbolsFromCache;
            }

            var styleSymbols = iffleyRoutesRecordContext.StyleSymbol.Select(CreateStyleSymbolResponse);
            cache.CacheListOfItems(styleSymbols, CacheItemPriority.High);

            return styleSymbols;
        }

        public IEnumerable<StyleSymbolResponse> GetStyleSymbolsOnProblem(int problemId)
        {
            var problem = iffleyRoutesRecordContext
                .Problem
                .Include(problemDbo => problemDbo.ProblemStyleSymbols)
                .ThenInclude(problemStyleSymbol => problemStyleSymbol.StyleSymbol)
                .SingleOrDefault(route => route.Id == problemId);

            return problem
                .ProblemStyleSymbols
                .Select(problemStyleSymbol => new StyleSymbolResponse()
                {
                    StyleSymbolId = problemStyleSymbol.StyleSymbolId,
                    Name = problemStyleSymbol.StyleSymbol.Name,
                    Description = problemStyleSymbol.StyleSymbol.Description
                });
        }

        public void AddProblemStyleSymbolsToDatabase(IEnumerable<int> styleSymbolIds, int problemId)
        {
            if (styleSymbolIds != null)
            {
                ValidateAddProblemStyleSymbolsRequest(styleSymbolIds, problemId);

                foreach (int styleSymbolId in styleSymbolIds)
                {
                    iffleyRoutesRecordContext.ProblemStyleSymbol.Add(new ProblemStyleSymbol()
                    {
                        StyleSymbolId = styleSymbolId,
                        ProblemId = problemId
                    });
                }
            }
        }

        private void ValidateAddProblemStyleSymbolsRequest(IEnumerable<int> styleSymbolIds, int problemId)
        {
            if (styleSymbolIds is null)
            {
                throw new ArgumentNullException(nameof(styleSymbolIds));
            }

            iffleyRoutesRecordContext.Problem.VerifyEntityWithIdExists(problemId);

            foreach (int styleSymbolId in styleSymbolIds)
            {
                iffleyRoutesRecordContext.StyleSymbol.VerifyEntityWithIdExists(styleSymbolId);
            }
        }

        private StyleSymbolResponse CreateStyleSymbolResponse(StyleSymbol styleSymbol)
        {
            if (styleSymbol is null)
            {
                throw new ArgumentNullException(nameof(styleSymbol));
            }

            return new StyleSymbolResponse()
            {
                StyleSymbolId = styleSymbol.Id,
                Name = styleSymbol.Name,
                Description = styleSymbol.Description
            };
        }
    }
}
