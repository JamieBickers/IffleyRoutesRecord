using System.Collections.Generic;

namespace IffleyRoutesRecord.Logic.Entities
{
    public class HoldRule : BaseNamedEntity
    {
        public string Description { get; set; }

        public List<ProblemHoldRule> ProblemHoldRules { get; private set; }
    }
}
