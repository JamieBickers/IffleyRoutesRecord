using System.Collections.Generic;
using IffleyRoutesRecord.DTOs;

namespace IffleyRoutesRecord.Logic
{
    public interface IStyleSymbolManager
    {
        IEnumerable<StyleSymbolDto> GetStyleSymbolsOnProblem(int problemId);
        void AddProblemStyleSymbolsToDatabase(IEnumerable<int> styleSymbolIds, int problemId);
    }
}