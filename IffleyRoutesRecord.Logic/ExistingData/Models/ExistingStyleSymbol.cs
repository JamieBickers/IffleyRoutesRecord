﻿using System.Diagnostics.CodeAnalysis;

namespace IffleyRoutesRecord.Logic.ExistingData.Models
{
    [SuppressMessage("Microsoft.Performance", "CA1812")]
    internal class ExistingStyleSymbol
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
