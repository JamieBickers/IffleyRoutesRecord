using System.Collections.Generic;

namespace IffleyRoutesRecord.Logic.Entities
{
    public abstract class BaseGrade
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Rank { get; set; }

        public List<Problem> Problems { get; set; }
    }
}
