﻿using System.Collections.Generic;

namespace IffleyRoutesRecord.Entities
{
    public class ProblemHold
    {
        public int Id { get; set; }
        public int Position { get; set; }
        public bool IsStandingStartHold { get; set; }
        public int ProblemId { get; set; }
        public int HoldId { get; set; }

        public Problem Problem { get; set; }
        public Hold Hold { get; set; }

        public List<ProblemHoldRule> ProblemHoldRules { get; set; }
    }
}
