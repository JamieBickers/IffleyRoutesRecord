namespace IffleyRoutesRecord.Models.Entities
{
    public class ProblemIssue : BaseEntity
    {
        public int ProblemId { get; set; }
        public string Description { get; set; }
        public string LoggedBy { get; set; }

        public Problem Problem { get; set; }
    }
}
