using IffleyRoutesRecord.DTOs;
using IffleyRoutesRecord.Entities;
using IffleyRoutesRecord.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IffleyRoutesRecord.Logic
{
    public class StyleSymbolManager : IStyleSymbolManager
    {
        private readonly IffleyRoutesRecordContext iffleyRoutesRecordContext;

        public StyleSymbolManager(IffleyRoutesRecordContext iffleyRoutesRecordContext)
        {
            this.iffleyRoutesRecordContext = iffleyRoutesRecordContext;
        }

        public void AddProblemStyleSymbolsToDatabase(IEnumerable<int> styleSymbolIds, int problemId)
        {
            if (styleSymbolIds != null)
            {
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

        public IEnumerable<StyleSymbolDto> GetStyleSymbolsOnProblem(int problemId)
        {
            var problem = iffleyRoutesRecordContext
                .Problem
                .Include(problemDbo => problemDbo.ProblemStyleSymbols)
                .ThenInclude(problemStyleSymbol => problemStyleSymbol.StyleSymbol)
                .SingleOrDefault(route => route.Id == problemId);

            return problem
                .ProblemStyleSymbols
                .Select(problemStyleSymbol => new StyleSymbolDto()
                {
                    StyleSymbolId = problemStyleSymbol.StyleSymbolId,
                    Name = problemStyleSymbol.StyleSymbol.Name,
                    Description = problemStyleSymbol.StyleSymbol.Description
                });
        }
    }
}
