using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IffleyRoutesRecord.Models.DTOs.Responses
{
    public class TechGradeResponse : BaseGradeResponse
    {
        public TechGradeResponse(int gradeId, string name, int rank, int globalGrade)
            : base(gradeId, name, rank, globalGrade)
        {

        }
    }
}
