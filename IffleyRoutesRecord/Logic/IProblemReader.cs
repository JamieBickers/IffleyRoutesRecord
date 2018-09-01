using System.Collections.Generic;
using IffleyRoutesRecord.DTOs;

namespace IffleyRoutesRecord.Logic
{
    public interface IProblemReader
    {
        ProblemDto GetProblem(int problemId);
        IEnumerable<ProblemDto> GetProblems();
    }
}