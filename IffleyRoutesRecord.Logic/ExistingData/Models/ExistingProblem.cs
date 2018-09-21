using System;
using System.Collections.Generic;
using System.Text;

namespace IffleyRoutesRecord.Logic.ExistingData.Models
{
    internal class ExistingProblem
    {
        public string Name { get; set; }
        public IEnumerable<string> Grades { get; set; }
        public IEnumerable<ExistingProblemHold> Holds { get; set; }
        public IEnumerable<string> Rules { get; set; }
    }
}