namespace IffleyRoutesRecord.Models.Entities
{
    public class ProblemIssue : BaseEntity
    {
        public int ProblemId { get; set; }
        public string Description { get; set; }
        public string SubmittedBy { get; set; }

        public Problem Problem { get; set; }
    }
}
