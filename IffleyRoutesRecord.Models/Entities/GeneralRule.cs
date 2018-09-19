using System.Collections.Generic;

namespace IffleyRoutesRecord.Models.Entities
{
    public class GeneralRule : BaseNamedEntity
    {
        public string Description { get; set; }

        public ICollection<ProblemRule> ProblemRules { get; private set; }
    }
}
