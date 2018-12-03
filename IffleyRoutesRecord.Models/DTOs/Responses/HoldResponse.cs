namespace IffleyRoutesRecord.Models.DTOs.Responses
{
    public class HoldResponse
    {
        public int HoldId { get; set; }
        public string Name { get; set; }
        public int? ParentHoldId { get; set; }
    }
}
