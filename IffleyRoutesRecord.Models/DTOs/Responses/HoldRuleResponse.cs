namespace IffleyRoutesRecord.Models.DTOs.Responses
{
    public class HoldRuleResponse
    {
        public int HoldRuleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public HoldRuleResponse(int holdRuleId, string name, string description)
        {
            HoldRuleId = holdRuleId;
            Name = name;
            Description = description;
        }
    }
}