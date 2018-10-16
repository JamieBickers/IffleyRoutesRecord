using IffleyRoutesRecord.Logic.Interfaces;
using IffleyRoutesRecord.Models.DTOs.Requests;
using IffleyRoutesRecord.Models.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace IffleyRoutesRecord.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProblemController : Controller
    {
        private readonly IProblemReader problemReader;
        private readonly IProblemCreator problemCreator;

        public ProblemController(IProblemReader problemReader, IProblemCreator problemCreator)
        {
            this.problemReader = problemReader;
            this.problemCreator = problemCreator;
        }

        /// <summary>
        /// Gets a list of all problems.
        /// </summary>
        /// <returns>The full list of problems</returns>
        /// <response code="200">The full list of problems</response>
        /// <response code="500">Unexpected error</response>
        [HttpGet]
        [ResponseCache(Duration = 10)]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult<IEnumerable<ProblemResponse>> GetProblems()
        {
            return problemReader.GetProblems().ToList();
        }

        /// <summary>
        /// Gets a problem by its ID.
        /// </summary>
        /// <param name="problemId">The ID of the problem to return</param>
        /// <returns>The problem with the given ID</returns>
        /// <response code="200">The requested problem</response>
        /// <response code="404">No problem with the provided ID was found</response>
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
        /// Creates a problem along with any additional rules needed.
        /// </summary>
        /// <param name="problem">The problem to be created</param>
        /// <returns>The created problem</returns>
        /// <response code="201">The created problem</response>
        /// <response code="404">One of the provided IDs was not found</response>
        /// <response code="409">One of the provided names for the problem or a rule already exists</response>
        /// <response code="500">Unexpected error</response>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(201)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public ActionResult<ProblemResponse> CreateProblem(CreateProblemRequest problem)
        {
            var createdProblem = problemCreator.CreateProblem(problem);
            return CreatedAtRoute(new { problemId = createdProblem.ProblemId }, createdProblem);
        }
    }
}