using System;
using System.Collections.Generic;

namespace IffleyRoutesRecord.Models.Entities
{
    public class Problem : BaseNamedEntity
    {
        public string Description { get; set; }
        public int? TechGradeId { get; set; }
        public int? BGradeId { get; set; }
        public int? PoveyGradeId { get; set; }
        public int? FurlongGradeId { get; set; }
        public DateTimeOffset? DateSet { get; set; }
        public string FirstAscent { get; set; }

        public TechGrade TechGrade { get; set; }
        public BGrade BGrade { get; set; }
        public PoveyGrade PoveyGrade { get; set; }
        public FurlongGrade FurlongGrade { get; set; }
        public bool Verified { get; set; }

        public ICollection<ProblemHold> ProblemHolds { get; set; }
        public ICollection<ProblemRule> ProblemRules { get; set; }
        public ICollection<ProblemStyleSymbol> ProblemStyleSymbols { get; set; }
    }
}
