using IffleyRoutesRecord.Logic.DTOs.Responses;
using System.Collections.Generic;

namespace IffleyRoutesRecord.Logic.Interfaces
{
    public interface IStyleSymbolManager
    {
        StyleSymbolResponse GetStyleSymbol(int styleSymbolId);
        IEnumerable<StyleSymbolResponse> GetStyleSymbols();
        IEnumerable<StyleSymbolResponse> GetStyleSymbolsOnProblem(int problemId);
    }
}