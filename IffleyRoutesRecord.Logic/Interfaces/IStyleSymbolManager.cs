using IffleyRoutesRecord.Logic.DTOs.Sent;
using System.Collections.Generic;

namespace IffleyRoutesRecord.Logic.Interfaces
{
    public interface IStyleSymbolManager
    {
        StyleSymbolDto GetStyleSymbol(int styleSymbolId);
        IEnumerable<StyleSymbolDto> GetStyleSymbols();
        IEnumerable<StyleSymbolDto> GetStyleSymbolsOnProblem(int problemId);
        void AddProblemStyleSymbolsToDatabase(IEnumerable<int> styleSymbolIds, int problemId);
    }
}