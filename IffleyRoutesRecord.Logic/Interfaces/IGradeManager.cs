using IffleyRoutesRecord.Logic.DTOs.Sent;

namespace IffleyRoutesRecord.Logic.Interfaces
{
    public interface IGradeManager
    {
        BGradeDto GetBGradeOnProblem(int problemId);
        FurlongGradeDto GetFurlongGradeOnProblem(int problemId);
        PoveyGradeDto GetPoveyGradeOnProblem(int problemId);
        TechGradeDto GetTechGradeOnProblem(int problemId);
    }
}