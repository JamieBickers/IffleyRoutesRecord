using System;
using System.Collections.Generic;

namespace IffleyRoutesRecord.Models.DTOs.Responses
{
    public class ProblemResponse
    {
        public ProblemResponse(int problemId, string name,  IList<HoldOnProblemResponse> holds)
        {
            ProblemId = problemId;
            Name = name;
            Holds = holds;
        }

        public int ProblemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SetBy { get; set; }
        public DateTimeOffset? DateSet { get; set; }
        public string FirstAscent { get; set; }

        public TechGradeResponse TechGrade { get; set; }
        public BGradeResponse BGrade { get; set; }
        public PoveyGradeResponse PoveyGrade { get; set; }
        public FurlongGradeResponse FurlongGrade { get; set; }

        public IList<HoldOnProblemResponse> Holds { get; set; }
        public IEnumerable<ProblemRuleResponse> Rules { get; set; }
        public IEnumerable<StyleSymbolResponse> StyleSymbols { get; set; }
    }
}