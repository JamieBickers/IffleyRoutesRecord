using System.Collections.Generic;

namespace IffleyRoutesRecord.Entities
{
    public class Hold
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentHoldId { get; set; }
        public Hold ParentHold { get; set; }

        public List<ProblemHold> ProblemHolds { get; set; }
        public List<Hold> ParentHolds { get; set; }
    }
}
