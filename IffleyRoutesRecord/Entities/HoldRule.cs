using System.Collections.Generic;

namespace IffleyRoutesRecord.Entities
{
    public class HoldRule
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<ProblemHoldRule> ProblemHoldRules { get; set; }
    }
}
