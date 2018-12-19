using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace IffleyRoutesRecord.Logic.ExistingData.Models
{
    [SuppressMessage("Microsoft.Performance", "CA1812")]
    internal class ExistingProblemHold
    {
        public string Hold { get; set; }
        public IEnumerable<string> Rules { get; set; }
        public bool IsStandingStartHold { get; set; }
    }
}