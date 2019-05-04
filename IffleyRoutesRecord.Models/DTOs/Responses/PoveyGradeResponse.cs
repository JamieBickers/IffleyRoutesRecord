using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IffleyRoutesRecord.Models.DTOs.Responses
{
    public class PoveyGradeResponse : BaseGradeResponse
    {
        public PoveyGradeResponse(int gradeId, string name, int rank, int globalGrade)
            : base(gradeId, name, rank, globalGrade)
        {

        }
    }
}
