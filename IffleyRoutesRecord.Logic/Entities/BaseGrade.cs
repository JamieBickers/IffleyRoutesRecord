﻿using System.Collections.Generic;

namespace IffleyRoutesRecord.Logic.Entities
{
    public abstract class BaseGrade : BaseNamedEntity
    {
        public int Rank { get; set; }

        public List<Problem> Problems { get; private set; }
    }
}
