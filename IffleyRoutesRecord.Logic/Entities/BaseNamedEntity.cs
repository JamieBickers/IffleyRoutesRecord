using System;
using System.Collections.Generic;
using System.Text;

namespace IffleyRoutesRecord.Logic.Entities
{
    public abstract class BaseNamedEntity : BaseEntity
    {
        public string Name { get; set; }
    }
}
