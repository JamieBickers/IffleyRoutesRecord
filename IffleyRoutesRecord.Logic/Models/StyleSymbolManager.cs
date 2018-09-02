using IffleyRoutesRecord.Logic.DataAccess;
using IffleyRoutesRecord.Logic.DTOs.Sent;
using IffleyRoutesRecord.Logic.Entities;
using IffleyRoutesRecord.Logic.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace IffleyRoutesRecord.Logic.Models
{
    public class StyleSymbolManager : IStyleSymbolManager
    {
        private readonly IffleyRoutesRecordContext iffleyRoutesRecordContext;

        public StyleSymbolManager(IffleyRoutesRecordContext iffleyRoutesRecordContext)
        {
            this.iffleyRoutesRecordContext = iffleyRoutesRecordContext;
        }

        public StyleSymbolDto GetStyleSymbol(int styleSymbolId)
        {
            var styleSymbol = iffleyRoutesRecordContext.StyleSymbol.Single(symbol => symbol.Id == styleSymbolId);
            return CreateStyleSymbolDto(styleSymbol);
        }

        public IEnumerable<StyleSymbolDto> GetStyleSymbols()
        {
            return iffleyRoutesRecordContext.StyleSymbol.Select(CreateStyleSymbolDto);
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

        private StyleSymbolDto CreateStyleSymbolDto(StyleSymbol styleSymbol)
        {
            return new StyleSymbolDto()
            {
                StyleSymbolId = styleSymbol.Id,
                Name = styleSymbol.Name,
                Description = styleSymbol.Description
            };
        }
    }
}
