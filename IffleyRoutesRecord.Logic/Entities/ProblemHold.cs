using System.Collections.Generic;

namespace IffleyRoutesRecord.Logic.Entities
{
    public class ProblemHold : BaseEntity
    {
        public int Position { get; set; }
        public bool IsStandingStartHold { get; set; }
        public int ProblemId { get; set; }
        public int HoldId { get; set; }

        public Problem Problem { get; set; }
        public Hold Hold { get; set; }

        public ICollection<ProblemHoldRule> ProblemHoldRules { get; set; }
    }
}
