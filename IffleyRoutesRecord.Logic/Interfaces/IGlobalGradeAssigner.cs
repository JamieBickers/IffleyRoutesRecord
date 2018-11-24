using System.Collections.Generic;
using IffleyRoutesRecord.Models.DTOs.Responses;

namespace IffleyRoutesRecord.Logic.Interfaces
{
    public interface IGlobalGradeAssigner
    {
        void AssignGlobalGrade(ProblemResponse problem);
        void AssignGlobalGrades(IEnumerable<ProblemResponse> problems);
    }
}