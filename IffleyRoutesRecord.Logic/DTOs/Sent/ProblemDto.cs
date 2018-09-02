using System;
using System.Collections.Generic;

namespace IffleyRoutesRecord.Logic.DTOs.Sent
{
    public class ProblemDto
    {
        public int ProblemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset? DateSet { get; set; }
        public string FirstAscent { get; set; }

        public TechGradeDto TechGrade { get; set; }
        public BGradeDto BGrade { get; set; }
        public PoveyGradeDto PoveyGrade { get; set; }
        public FurlongGradeDto FurlongGrade { get; set; }

        public IList<HoldOnProblemDto> Holds { get; set; }
        public IEnumerable<ProblemRuleDto> Rules { get; set; }
        public IEnumerable<StyleSymbolDto> StyleSymbols { get; set; }
    }
}