using System;
using System.Collections.Generic;
using System.Text;

namespace IffleyRoutesRecord.Logic.ExistingData
{
    internal class ExistingProblemHold
    {
        public string Hold { get; set; }
        public IEnumerable<string> Rules { get; set; }
        public bool IsStandingStartHold { get; set; }
    }
}