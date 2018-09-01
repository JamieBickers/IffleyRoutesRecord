using IffleyRoutesRecord.DTOs;

namespace IffleyRoutesRecord.Logic
{
    public interface IProblemCreator
    {
        ProblemDto CreateProblem(CreateProblemDto problem);
    }
}