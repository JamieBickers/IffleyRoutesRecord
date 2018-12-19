using System.Collections.Generic;

namespace IffleyRoutesRecord.Models.Entities
{
    public class StyleSymbol : BaseNamedEntity
    {
        public string Description { get; set; }

        public ICollection<ProblemStyleSymbol> ProblemStyleSymbols { get; private set; }
    }
}
