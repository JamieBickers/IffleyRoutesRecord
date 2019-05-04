namespace IffleyRoutesRecord.Models.DTOs.Responses
{
    public class HoldResponse
    {
        public int HoldId { get; set; }
        public string Name { get; set; }
        public int? ParentHoldId { get; set; }

        public HoldResponse(int holdId, string name, int? parentHoldId)
        {
            HoldId = holdId;
            Name = name;
            ParentHoldId = parentHoldId;
        }
    }
}
