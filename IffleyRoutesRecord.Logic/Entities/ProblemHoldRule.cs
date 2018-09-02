namespace IffleyRoutesRecord.Logic.Entities
{
    public class ProblemHoldRule
    {
        public int Id { get; set; }
        public int ProblemHoldId { get; set; }
        public int HoldRuleId { get; set; }

        public ProblemHold ProblemHold { get; set; }
        public HoldRule HoldRule { get; set; }
    }
}
