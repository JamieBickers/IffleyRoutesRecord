using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IffleyRoutesRecord.DTOs
{
    public class HoldDto
    {
        public int HoldId { get; set; }
        public string Name { get; set; }
        public bool IsStandingStartHold { get; set; }
        public IEnumerable<HoldRuleDto> HoldRules { get; set; }
    }
}
