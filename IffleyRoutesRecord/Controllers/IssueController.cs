using IffleyRoutesRecord.Logic.Interfaces;
using IffleyRoutesRecord.Models.DTOs.Requests;
using IffleyRoutesRecord.Models.DTOs.Responses;
using IffleyRoutesRecord.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace IffleyRoutesRecord.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssueController : Controller
    {
        private readonly IIssueManager issueManager;

        public IssueController(IIssueManager issueManager)
        {
            this.issueManager = issueManager;
        }

        /// <summary>
        /// Gets a list of all issues.
        /// </summary>
        /// <returns>The full list of issues</returns>
        /// <response code="200">The full list of issues</response>
        /// <response code="401">You must be logged on as an admin to do this.</response>
        /// <response code="500">Unexpected error</response>
        [HttpGet]
        [ResponseCache(Duration = 10)]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [Authorize(UserRoles.Admin)]
        public ActionResult<IEnumerable<Issue>> GetIssues()
        {
#if DEBUG
            return Ok(issueManager.GetIssues());
#else
            return StatusCode((int)HttpStatusCode.NotImplemented);
#endif
        }

        /// <summary>
        /// Gets a list of all problem issues.
        /// </summary>
        /// <returns>The full list of problem issues</returns>
        /// <response code="200">The full list of problem issues</response>
        /// <response code="401">You must be logged on as an admin to do this.</response>
        /// <response code="500">Unexpected error</response>
        [HttpGet("problem")]
        [ResponseCache(Duration = 10)]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [Authorize(UserRoles.Admin)]
        public ActionResult<IEnumerable<ProblemIssueResponse>> GetProblemIssues()
        {
            return Ok(issueManager.GetProblemIssues());
        }

        /// <summary>
        /// Creates a problem along with any additional rules needed. This is unavailable until authentication is set up.
        /// </summary>
        /// <param name="issue">The problem to be created</param>
        /// <returns>Status code indicating success</returns>
        /// <response code="204">Success</response>
        /// <response code="401">You must be a standard user to do this.</response>
        /// <response code="404">Invalid problem Id</response>
        /// <response code="500">Unexpected error</response>
        [HttpPost("problem")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(UserRoles.Standard)]
        public StatusCodeResult CreateProblemIssue(CreateProblemIssueRequest issue)
        {
            issueManager.CreateProblemIssue(issue, UserEmail);
            return new StatusCodeResult(204);
        }

        /// <summary>
        /// Creates a problem along with any additional rules needed. This is unavailable until authentication is set up.
        /// </summary>
        /// <param name="issue">The problem to be created</param>
        /// <returns>Status code indicating success</returns>
        /// <response code="204">Success</response>
        /// <response code="500">Unexpected error</response>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public StatusCodeResult CreateIssue(CreateIssueRequest issue)
        {
            issueManager.CreateIssue(issue, UserEmail);
            return new StatusCodeResult(204);
        }

        private string UserEmail => "Dummy email";
    }
}