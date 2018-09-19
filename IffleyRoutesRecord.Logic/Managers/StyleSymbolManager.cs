using IffleyRoutesRecord.Logic.DataAccess;
using IffleyRoutesRecord.Logic.Exceptions;
using IffleyRoutesRecord.Logic.Interfaces;
using IffleyRoutesRecord.Logic.StaticHelpers;
using IffleyRoutesRecord.Models.DTOs.Responses;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;

namespace IffleyRoutesRecord.Logic.Managers
{
    public class StyleSymbolManager : IStyleSymbolManager
    {
        private readonly IffleyRoutesRecordContext repository;
        private readonly IMemoryCache cache;

        public StyleSymbolManager(IffleyRoutesRecordContext repository, IMemoryCache cache)
        {
            this.repository = repository;
            this.cache = cache;
        }

        public StyleSymbolResponse GetStyleSymbol(int styleSymbolId)
        {
            if (cache.TryRetrieveItemWithId<StyleSymbolResponse>(
                styleSymbolId, cachedStyleSymbol => cachedStyleSymbol.StyleSymbolId, out var styleSymbolResponse))
            {
                return styleSymbolResponse;
            }

            var styleSymbol = repository.StyleSymbol.SingleOrDefault(symbol => symbol.Id == styleSymbolId);

            if (styleSymbol is null)
            {
                throw new EntityNotFoundException($"No style symbol with ID {styleSymbolId} was found.");
            }

            return Mapper.Map(styleSymbol);
        }

        public IEnumerable<StyleSymbolResponse> GetStyleSymbols()
        {
            if (cache.TryRetrieveAllItems<StyleSymbolResponse>(out var styleSymbolsFromCache))
            {
                return styleSymbolsFromCache;
            }

            var styleSymbols = repository.StyleSymbol.Select(Mapper.Map);
            cache.CacheListOfItems(styleSymbols, CacheItemPriority.High);

            return styleSymbols;
        }
    }
}
