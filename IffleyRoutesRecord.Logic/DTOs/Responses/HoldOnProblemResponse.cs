using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IffleyRoutesRecord.Logic.DTOs.Responses
{
    public class HoldOnProblemResponse
    {
        public int HoldId { get; set; }
        public string Name { get; set; }
        public bool IsStandingStartHold { get; set; }
        public IEnumerable<HoldRuleResponse> HoldRules { get; set; }
    }
}
