using System.Collections.Generic;

namespace IffleyRoutesRecord.Models.Entities
{
    public abstract class BaseGrade : BaseNamedEntity
    {
        public int Rank { get; set; }
        public int GlobalGrade { get; set; }

        public ICollection<Problem> Problems { get; private set; }
    }
}
