using IffleyRoutesRecord.Models.DTOs.Requests;

namespace IffleyRoutesRecord.Logic.Interfaces
{
    /// <summary>
    /// Validates problem related requests against the database
    /// </summary>
    public interface IProblemRequestValidator
    {
        /// <summary>
        /// Validates a CreateProblemRequest
        /// </summary>
        /// <param name="problem">The request to validate</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="EntityNotFoundException"></exception>
        /// <exception cref="EntityWithNameAlreadyExistsException"></exception>
        /// <exception cref="InternalEntityNotFoundException"></exception>
        void Validate(CreateProblemRequest problem);
    }
}