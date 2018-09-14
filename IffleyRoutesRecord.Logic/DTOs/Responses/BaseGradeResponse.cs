using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IffleyRoutesRecord.Logic.DTOs.Responses
{
    public abstract class BaseGradeResponse
    {
        public int GradeId { get; set; }
        public string Name { get; set; }
        public int Rank { get; set; }
    }
}
