namespace IffleyRoutesRecord.Models.DTOs.Responses
{
    public class StyleSymbolResponse
    {
        public int StyleSymbolId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public StyleSymbolResponse(int styleSymbolId, string name, string description)
        {
            StyleSymbolId = styleSymbolId;
            Name = name;
            Description = description;
        }
    }
}