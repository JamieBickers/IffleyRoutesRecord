using System.Collections.Generic;
using IffleyRoutesRecord.Models.DTOs.Responses;

namespace IffleyRoutesRecord.Logic.Interfaces
{
    /// <summary>
    /// Retreives problems from the database
    /// </summary>
    public interface IProblemReader
    {
        /// <summary>
        /// Gets the problem with the Given ID
        /// </summary>
        /// <param name="problemId">ID of the problem to get</param>
        /// <returns>The problem</returns>
        /// <exception cref="EntityNotFoundException"></exception>
        ProblemResponse GetProblem(int problemId);

        /// <summary>
        /// Gets all problems
        /// </summary>
        /// <returns>A list of all problems</returns>
        IEnumerable<ProblemResponse> GetProblems();
    }
}