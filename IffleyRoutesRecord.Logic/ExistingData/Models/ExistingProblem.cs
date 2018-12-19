using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace IffleyRoutesRecord.Logic.ExistingData.Models
{
    [SuppressMessage("Microsoft.Performance", "CA1812")]
    internal class ExistingProblem
    {
        public string Name { get; set; }
        public IEnumerable<string> Grades { get; set; }
        public IEnumerable<ExistingProblemHold> Holds { get; set; }
        public IEnumerable<string> Rules { get; set; }
    }
}