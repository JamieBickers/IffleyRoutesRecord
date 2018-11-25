namespace IffleyRoutesRecord.Models.DTOs.Responses
{
    public class ProblemIssueResponse
    {
        public int ProblemIssueId { get; set; }
        public string Description { get; set; }
        public string SubmittedBy { get; set; }

        public ProblemResponse Problem { get; set; }
    }
}
