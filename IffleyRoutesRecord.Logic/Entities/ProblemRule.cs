namespace IffleyRoutesRecord.Logic.Entities
{
    public class ProblemRule
    {
        public int Id { get; set; }
        public int ProblemId { get; set; }
        public int GeneralRuleId { get; set; }

        public Problem Problem { get; set; }
        public GeneralRule GeneralRule { get; set; }
    }
}
