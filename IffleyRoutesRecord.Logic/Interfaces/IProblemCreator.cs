using IffleyRoutesRecord.Logic.DTOs.Requests;
using IffleyRoutesRecord.Logic.DTOs.Responses;

namespace IffleyRoutesRecord.Logic.Interfaces
{
    public interface IProblemCreator
    {
        ProblemResponse CreateProblem(CreateProblemRequest problem);
    }
}