using System.Collections.Generic;

namespace IffleyRoutesRecord.Models.Entities
{
    public class HoldRule : BaseNamedEntity
    {
        public string Description { get; set; }

        public ICollection<ProblemHoldRule> ProblemHoldRules { get; private set; }
    }
}
