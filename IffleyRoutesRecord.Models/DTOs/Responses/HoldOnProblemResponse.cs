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

        public HoldOnProblemResponse(int holdId, string name, int? parentHoldId, bool isStandingStartHold, IEnumerable<HoldRuleResponse> holdRules)
        {
            HoldId = holdId;
            Name = name;
            ParentHoldId = parentHoldId;
            IsStandingStartHold = isStandingStartHold;
            HoldRules = holdRules;
        }
    }
}
