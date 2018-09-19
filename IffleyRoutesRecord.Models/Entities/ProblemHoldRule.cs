namespace IffleyRoutesRecord.Models.Entities
{
    public class ProblemHoldRule : BaseEntity
    {
        public int ProblemHoldId { get; set; }
        public int HoldRuleId { get; set; }

        public ProblemHold ProblemHold { get; set; }
        public HoldRule HoldRule { get; set; }
    }
}
