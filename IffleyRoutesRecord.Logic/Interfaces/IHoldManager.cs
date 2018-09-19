using IffleyRoutesRecord.Models.DTOs.Responses;
using System.Collections.Generic;

namespace IffleyRoutesRecord.Logic.Interfaces
{
    /// <summary>
    /// Basic CRUD operations for holds
    /// </summary>
    public interface IHoldManager
    {
        /// <summary>
        /// Gets the hold by its ID
        /// </summary>
        /// <param name="holdId">ID of the hold to get</param>
        /// <returns>The hold</returns>
        /// <exception cref="EntityNotFoundException"></exception>
        HoldResponse GetHold(int holdId);

        /// <summary>
        /// Gets all holds
        /// </summary>
        /// <returns>A list of all holds</returns>
        IEnumerable<HoldResponse> GetHolds();
    }
}