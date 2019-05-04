namespace IffleyRoutesRecord.Models.DTOs.Responses
{
    public class ProblemRuleResponse
    {
        public int ProblemRuleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ProblemRuleResponse(int problemRuleId, string name, string description)
        {
            ProblemRuleId = problemRuleId;
            Name = name;
            Description = description;
        }
    }
}