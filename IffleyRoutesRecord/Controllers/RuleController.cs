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
    public class RuleController : BaseController
    {
        private readonly RuleManager ruleManager;

        public RuleController(RuleManager ruleManager, IConfiguration configuration)
            : base(configuration)
        {
            this.ruleManager = ruleManager;
        }

        /// <summary>
        /// Gets a problem rule by its ID.
        /// </summary>
        /// <param name="ruleId">The ID of the problem rule to return</param>
        /// <returns>The problem rule with the given ID</returns>
        /// <response code="200">The requested problem rule</response>
        /// <response code="404">No problem rule with the provided ID was found</response>
        /// <response code="500">Unexpected error</response>
        [HttpGet("problem/{ruleId}")]
        [ResponseCache(Duration = 10)]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<ProblemRuleResponse> GetProblemRule(int ruleId)
        {
            return ruleManager.GetProblemRule(ruleId);
        }

        /// <summary>
        /// Gets a list of all problem rules.
        /// </summary>
        /// <returns>The full list of problem rules</returns>
        /// <response code="200">The full list of problem rules</response>
        /// <response code="500">Unexpected error</response>
        [HttpGet("problem")]
        [ResponseCache(Duration = 10)]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult<IEnumerable<ProblemRuleResponse>> GetProblemRules()
        {
            return ruleManager.GetAllProblemRules().ToList();
        }

        /// <summary>
        /// Gets a hold rule by its ID.
        /// </summary>
        /// <param name="ruleId">The ID of the hold rule to return</param>
        /// <returns>The hold rule with the given ID</returns>
        /// <response code="200">The requested hold rule</response>
        /// <response code="404">No hold rule with the provided ID was found</response>
        /// <response code="500">Unexpected error</response>
        [HttpGet("hold/{ruleId}")]
        [ResponseCache(Duration = 10)]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<HoldRuleResponse> GetHoldRule(int ruleId)
        {
            return ruleManager.GetHoldRule(ruleId);
        }

        /// <summary>
        /// Gets a list of all hold rules.
        /// </summary>
        /// <returns>The full list of hold rules</returns>
        /// <response code="200">The full list of hold rules</response>
        /// <response code="500">Unexpected error</response>
        [HttpGet("hold")]
        [ResponseCache(Duration = 10)]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult<IEnumerable<HoldRuleResponse>> GetHoldRules()
        {
            return ruleManager.GetAllHoldRules().ToList();
        }
    }
}
