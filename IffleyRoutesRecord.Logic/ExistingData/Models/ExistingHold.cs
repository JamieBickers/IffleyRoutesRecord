using System;
using System.Collections.Generic;
using System.Text;

namespace IffleyRoutesRecord.Logic.ExistingData.Models
{
    internal class ExistingHold
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentHoldId { get; set; }
    }
}
