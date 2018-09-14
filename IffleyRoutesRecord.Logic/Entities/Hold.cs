using System.Collections.Generic;

namespace IffleyRoutesRecord.Logic.Entities
{
    public class Hold : BaseNamedEntity
    {
        public int? ParentHoldId { get; set; }
        public Hold ParentHold { get; set; }

        public List<ProblemHold> ProblemHolds { get; private set; }
        public List<Hold> ParentHolds { get; private set; }
    }
}
