using IffleyRoutesRecord.Logic.DataAccess;
using IffleyRoutesRecord.Logic.Exceptions;
using IffleyRoutesRecord.Logic.Interfaces;
using IffleyRoutesRecord.Logic.StaticHelpers;
using IffleyRoutesRecord.Models.DTOs.Responses;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;

namespace IffleyRoutesRecord.Logic.Managers
{
    /// <summary>
    /// Reads holds
    /// </summary>
    public class HoldManager
    {
        private readonly IffleyRoutesRecordContext repository;
        private readonly IMemoryCache cache;
        private readonly RuleManager ruleManager;

        public HoldManager(IffleyRoutesRecordContext repository, IMemoryCache cache, RuleManager ruleManager)
        {
            this.repository = repository;
            this.cache = cache;
            this.ruleManager = ruleManager;
        }

        /// <summary>
        /// Gets the hold by its ID
        /// </summary>
        /// <param name="holdId">ID of the hold to get</param>
        /// <returns>The hold</returns>
        /// <exception cref="EntityNotFoundException"></exception>
        public HoldResponse GetHold(int holdId)
        {
            if (cache.TryRetrieveItemWithId<HoldResponse>(holdId, cachedHoldResponse => cachedHoldResponse.HoldId, out var holdResponse))
            {
                return holdResponse;
            }

            var hold = repository.Hold.SingleOrDefault(holdDbo => holdDbo.Id == holdId);

            if (hold is null)
            {
                throw new EntityNotFoundException($"No hold with ID {holdId} was found.");
            }

            return Mapper.Map(hold);
        }

        /// <summary>
        /// Gets all holds
        /// </summary>
        /// <returns>A list of all holds</returns>
        public IEnumerable<HoldResponse> GetHolds()
        {
            if (cache.TryRetrieveAllItems<HoldResponse>(out var holdResponses))
            {
                return holdResponses;
            }

            var holds = repository.Hold.Select(Mapper.Map);
            cache.CacheListOfItems(holds, CacheItemPriority.High);

            return holds;
        }
    }
}
