using System.Collections.Generic;

namespace IffleyRoutesRecord.Logic.Entities
{
    public class GeneralRule : BaseNamedEntity
    {
        public string Description { get; set; }

        public List<ProblemRule> ProblemRules { get; private set; }
    }
}
