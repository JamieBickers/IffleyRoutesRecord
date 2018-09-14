using System.Collections.Generic;

namespace IffleyRoutesRecord.Logic.Entities
{
    public class StyleSymbol : BaseNamedEntity
    {
        public string Description { get; set; }
        public string SymbolFilePath { get; set; }

        public ICollection<ProblemStyleSymbol> ProblemStyleSymbols { get; private set; }
    }
}
