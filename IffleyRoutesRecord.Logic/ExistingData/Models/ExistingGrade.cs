using System.Diagnostics.CodeAnalysis;

namespace IffleyRoutesRecord.Logic.ExistingData.Models
{
    [SuppressMessage("Microsoft.Performance", "CA1812")]
    internal class ExistingGrade
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Rank { get; set; }
        public int? GlobalGrade { get; set; }
    }
}
