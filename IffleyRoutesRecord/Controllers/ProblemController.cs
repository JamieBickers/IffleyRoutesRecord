using IffleyRoutesRecord.Logic.DTOs.Requests;
using IffleyRoutesRecord.Logic.DTOs.Responses;
using IffleyRoutesRecord.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;

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

        [HttpGet]
        [ResponseCache(Duration = 300)]
        public ActionResult<IEnumerable<ProblemResponse>> GetProblems()
        {
            return problemReader.GetProblems().ToList();
        }

        [HttpGet("{problemId}")]
        [ResponseCache(Duration = 900)]
        public ActionResult<ProblemResponse> GetProblem(int problemId)
        {
            var problem = problemReader.GetProblem(problemId);

            if (problem is null)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }
            else
            {
                return problem;
            }
        }

        [HttpPost]
        public ActionResult<ProblemResponse> CreateProblem(CreateProblemRequest problem)
        {
            var createdProblem = problemCreator.CreateProblem(problem);

            if (createdProblem is null)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }

            return CreatedAtRoute(new { problemId = createdProblem.ProblemId }, createdProblem);
        }
    }
}