using System.Collections.Generic;
using System.Linq;
using IffleyRoutesRecord.Logic.Managers;
using IffleyRoutesRecord.Models.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace IffleyRoutesRecord.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoldController : BaseController
    {
        private readonly HoldManager holdManager;

        public HoldController(HoldManager holdManager, IConfiguration configuration)
            : base(configuration)
        {
            this.holdManager = holdManager;
        }

        /// <summary>
        /// Gets a list of all holds.
        /// </summary>
        /// <returns>The full list of holds</returns>
        /// <response code="200">The full list of holds</response>
        /// <response code="500">Unexpected error</response>
        [HttpGet]
        [ResponseCache(Duration = 60 * 60 * 24)]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult<IEnumerable<HoldResponse>> GetHolds()
        {
            return holdManager.GetHolds().ToList();
        }

        /// <summary>
        /// Gets a hold by its ID.
        /// </summary>
        /// <param name="holdId">The ID of the hold to return</param>
        /// <returns>The hold with the given ID</returns>
        /// <response code="200">The requested hold</response>
        /// <response code="404">No hold with the provided ID was found</response>
        /// <response code="500">Unexpected error</response>
        [HttpGet("{holdId}")]
        [ResponseCache(Duration = 60 * 60 * 24)]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<HoldResponse> GetHold(int holdId)
        {
            return holdManager.GetHold(holdId);
        }
    }
}
