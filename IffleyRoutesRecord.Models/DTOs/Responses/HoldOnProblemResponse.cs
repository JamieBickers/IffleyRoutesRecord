using System.Collections.Generic;

namespace IffleyRoutesRecord.Models.DTOs.Responses
{
    public class HoldOnProblemResponse
    {
        public int HoldId { get; set; }
        public string Name { get; set; }
        public int? ParentHoldId { get; set; }
        public bool IsStandingStartHold { get; set; }
        public IEnumerable<HoldRuleResponse> HoldRules { get; set; }
    }
}
