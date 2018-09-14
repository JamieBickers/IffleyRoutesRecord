﻿using IffleyRoutesRecord.Logic.DataAccess;
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

            var styleSymbol = repository.StyleSymbol.Single(symbol => symbol.Id == styleSymbolId);
            return CreateStyleSymbolResponse(styleSymbol);
        }

        public IEnumerable<StyleSymbolResponse> GetStyleSymbols()
        {
            if (cache.TryRetrieveAllItems<StyleSymbolResponse>(out var styleSymbolsFromCache))
            {
                return styleSymbolsFromCache;
            }

            var styleSymbols = repository.StyleSymbol.Select(CreateStyleSymbolResponse);
            cache.CacheListOfItems(styleSymbols, CacheItemPriority.High);

            return styleSymbols;
        }

        public IEnumerable<StyleSymbolResponse> GetStyleSymbolsOnProblem(int problemId)
        {
            var problem = repository
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
