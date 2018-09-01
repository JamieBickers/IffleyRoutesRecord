using IffleyRoutesRecord.DTOs;
using IffleyRoutesRecord.Entities;

namespace IffleyRoutesRecord.Logic
{
    public interface IGradeManager
    {
        BGradeDto GetBGradeOnProblem(int problemId);
        FurlongGradeDto GetFurlongGradeOnProblem(int problemId);
        PoveyGradeDto GetPoveyGradeOnProblem(int problemId);
        TechGradeDto GetTechGradeOnProblem(int problemId);
    }
}