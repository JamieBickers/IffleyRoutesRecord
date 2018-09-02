using System.Collections.Generic;
using IffleyRoutesRecord.Logic.DTOs.Sent;

namespace IffleyRoutesRecord.Logic.Interfaces
{
    public interface IProblemReader
    {
        ProblemDto GetProblem(int problemId);
        IEnumerable<ProblemDto> GetProblems();
    }
}