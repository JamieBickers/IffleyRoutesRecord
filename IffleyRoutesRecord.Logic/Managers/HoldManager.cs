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
    public class HoldManager : IHoldManager
    {
        private readonly IffleyRoutesRecordContext repository;
        private readonly IMemoryCache cache;
        private readonly IRuleManager ruleManager;

        public HoldManager(IffleyRoutesRecordContext repository, IMemoryCache cache, IRuleManager ruleManager)
        {
            this.repository = repository;
            this.cache = cache;
            this.ruleManager = ruleManager;
        }

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
