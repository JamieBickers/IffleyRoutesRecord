using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IffleyRoutesRecord.Logic.DTOs.Sent
{
    public class HoldOnProblemDto
    {
        public int HoldId { get; set; }
        public string Name { get; set; }
        public bool IsStandingStartHold { get; set; }
        public IEnumerable<HoldRuleDto> HoldRules { get; set; }
    }
}
