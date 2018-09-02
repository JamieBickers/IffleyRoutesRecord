using System.Collections.Generic;

namespace IffleyRoutesRecord.Logic.Entities
{
    public class StyleSymbol
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SymbolFilePath { get; set; }

        public List<ProblemStyleSymbol> ProblemStyleSymbols { get; set; }
    }
}
