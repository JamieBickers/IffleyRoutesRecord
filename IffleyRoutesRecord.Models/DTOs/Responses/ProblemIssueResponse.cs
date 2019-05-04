namespace IffleyRoutesRecord.Models.DTOs.Responses
{
    public class ProblemIssueResponse
    {
        public int ProblemIssueId { get; set; }
        public string Description { get; set; }
        public string SubmittedBy { get; set; }

        public ProblemResponse Problem { get; set; }

        public ProblemIssueResponse(int problemIssueId, string description, string submittedBy, ProblemResponse problem)
        {
            ProblemIssueId = problemIssueId;
            Description = description;
            SubmittedBy = submittedBy;
            Problem = problem;
        }
    }
}
