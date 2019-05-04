using IffleyRoutesRecord.Auth;
using IffleyRoutesRecord.Logic.Interfaces;
using IffleyRoutesRecord.Models.DTOs.Requests;
using IffleyRoutesRecord.Models.DTOs.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace IffleyRoutesRecord.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProblemController : BaseController
    {
        private readonly IProblemReader problemReader;
        private readonly IProblemCreator problemCreator;

        public ProblemController(IProblemReader problemReader, IProblemCreator problemCreator, IConfiguration configuration)
            : base(configuration)
        {
            this.problemReader = problemReader;
            this.problemCreator = problemCreator;
        }

        /// <summary>
        /// Gets a list of all verified problems.
        /// </summary>
        /// <returns>The full list of problems</returns>
        /// <response code="200">The full list of problems</response>
        /// <response code="500">Unexpected error</response>
        [HttpGet]
        [ResponseCache(Duration = 60)]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult<IEnumerable<ProblemResponse>> GetProblems()
        {
            return problemReader.GetProblems().ToList();
        }

        /// <summary>
        /// Gets a verified problem by its ID.
        /// </summary>
        /// <param name="problemId">The ID of the problem to return</param>
        /// <returns>The problem with the given ID</returns>
        /// <response code="200">The requested problem</response>
        /// <response code="404">No verified problem with the provided ID was found</response>
        /// <response code="500">Unexpected error</response>
        [HttpGet("{problemId}")]
        [ResponseCache(Duration = 10)]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<ProblemResponse> GetProblem(int problemId)
        {
            return problemReader.GetProblem(problemId);
        }

        /// <summary>
        /// Gets a list of all unverified problems.
        /// </summary>
        /// <returns>The full list of unverified problems</returns>
        /// <response code="200">The full list of unverified problems</response>
        /// <response code="401">You must be logged on as an admin to do this</response>
        /// <response code="500">Unexpected error</response>
        [HttpGet("unverified")]
        [ResponseCache(Duration = 10)]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [Authorize(UserRoles.Admin)]
        public ActionResult<IEnumerable<ProblemResponse>> GetUnverifiedProblems()
        {
            return problemReader.GetUnverifiedProblems().ToList();
        }

        /// <summary>
        /// Creates a problem along with any additional rules needed.
        /// </summary>
        /// <param name="problem">The problem to be created</param>
        /// <returns>The created problem</returns>
        /// <response code="201">The created problem</response>
        /// <response code="401">You must be logged on to do this</response>
        /// <response code="404">One of the provided IDs was not found</response>
        /// <response code="409">One of the provided names for the problem or a rule already exists</response>
        /// <response code="500">Unexpected error</response>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        [Authorize(UserRoles.Standard)]
        public ActionResult<ProblemResponse> CreateUnverifiedProblem(CreateProblemRequest problem)
        {
            var createdProblem = problemCreator.CreateUnverifiedProblem(problem, UserEmail);
            return CreatedAtRoute(new { problemId = createdProblem.ProblemId }, createdProblem);
        }

        /// <summary>
        /// Sets the verified flag on a problem to true.
        /// </summary>
        /// <param name="problemId">The problem to be verifiedd</param>
        /// <returns>The created problem</returns>
        /// <response code="201">The problem</response>
        /// <response code="401">You must be logged on as an admin to do this</response>
        /// <response code="404">Problem not found</response>
        /// <response code="500">Unexpected error</response>
        [HttpPost("verify/{problemId}")]
        [Produces("application/json")]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(UserRoles.Admin)]
        public ActionResult<ProblemResponse> VerifyProblem(int problemId)
        {
            var verifiedProblem = problemCreator.VerifyProblem(problemId);
            return CreatedAtRoute(new { problemId = verifiedProblem.ProblemId }, verifiedProblem);
        }
    }
}