using System.Collections.Generic;

namespace IffleyRoutesRecord.Entities
{
    public abstract class BaseGrade
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Rank { get; set; }

        public List<Problem> Problems { get; set; }
    }
}
