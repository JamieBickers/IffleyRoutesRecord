﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IffleyRoutesRecord.DTOs
{
    public abstract class BaseGradeDto
    {
        public int GradeId { get; set; }
        public string Name { get; set; }
        public int Rank { get; set; }
    }
}
