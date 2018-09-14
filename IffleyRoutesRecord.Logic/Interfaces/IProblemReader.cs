using System.Collections.Generic;
using IffleyRoutesRecord.Logic.DTOs.Responses;

namespace IffleyRoutesRecord.Logic.Interfaces
{
    public interface IProblemReader
    {
        ProblemResponse GetProblem(int problemId);
        IEnumerable<ProblemResponse> GetProblems();
    }
}