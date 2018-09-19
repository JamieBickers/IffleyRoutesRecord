using System.Collections.Generic;

namespace IffleyRoutesRecord.Models.Entities
{
    public class Hold : BaseNamedEntity
    {
        public int? ParentHoldId { get; set; }
        public Hold ParentHold { get; set; }

        public ICollection<ProblemHold> ProblemHolds { get; private set; }
        public ICollection<Hold> ParentHolds { get; private set; }
    }
}
