using IffleyRoutesRecord.Models.DTOs.Requests;
using IffleyRoutesRecord.Models.DTOs.Responses;

namespace IffleyRoutesRecord.Logic.Interfaces
{
    /// <summary>
    /// Creates problems
    /// </summary>
    public interface IProblemCreator
    {
        /// <summary>
        /// Creates a problem
        /// </summary>
        /// <param name="problem">Problem to create</param>
        /// <returns>The created problem</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="EntityNotFoundException"></exception>
        /// <exception cref="EntityWithNameAlreadyExistsException"></exception>
        /// <exception cref="InternalEntityNotFoundException"></exception>
        ProblemResponse CreateUnverifiedProblem(CreateProblemRequest problem);

        /// <summary>
        /// Sets the verified flag on a problem to true.
        /// </summary>
        /// <param name="problemId">ID of the problem to set the verified flag on</param>
        /// <exception cref="EntityNotFoundException"></exception>
        /// <exception cref="InternalEntityNotFoundException"></exception>
        ProblemResponse VerifyProblem(int problemId);
    }
}