using IffleyRoutesRecord.Logic.DTOs.Responses;
using System.Collections.Generic;

namespace IffleyRoutesRecord.Logic.Interfaces
{
    public interface IGradeManager
    {
        TechGradeResponse GetTechGrade(int gradeId);
        BGradeResponse GetBGrade(int gradeId);
        PoveyGradeResponse GetPoveyGrade(int gradeId);
        FurlongGradeResponse GetFurlongGrade(int gradeId);

        IList<TechGradeResponse> GetTechGrades();
        IList<BGradeResponse> GetBGrades();
        IList<PoveyGradeResponse> GetPoveyGrades();
        IList<FurlongGradeResponse> GetFurlongGrades();

        BGradeResponse GetBGradeOnProblem(int problemId);
        FurlongGradeResponse GetFurlongGradeOnProblem(int problemId);
        PoveyGradeResponse GetPoveyGradeOnProblem(int problemId);
        TechGradeResponse GetTechGradeOnProblem(int problemId);
    }
}