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
        ProblemResponse CreateProblem(CreateProblemRequest problem);
    }
}