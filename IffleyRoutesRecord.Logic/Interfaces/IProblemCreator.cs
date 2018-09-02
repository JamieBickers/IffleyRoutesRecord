using IffleyRoutesRecord.Logic.DTOs.Received;
using IffleyRoutesRecord.Logic.DTOs.Sent;

namespace IffleyRoutesRecord.Logic.Interfaces
{
    public interface IProblemCreator
    {
        ProblemDto CreateProblem(CreateProblemDto problem);
    }
}