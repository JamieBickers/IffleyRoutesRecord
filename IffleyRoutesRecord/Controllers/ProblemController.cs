using IffleyRoutesRecord.DTOs;
using IffleyRoutesRecord.Logic;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace IffleyRoutesRecord.Controllers
{
    [Route("problem")]
    public class ProblemController : Controller
    {
        private readonly IProblemReader problemReader;
        private readonly IProblemCreator problemCreator;

        public ProblemController(IProblemReader problemReader, IProblemCreator problemCreator)
        {
            this.problemReader = problemReader;
            this.problemCreator = problemCreator;
        }

        [HttpGet("problems")]
        public IActionResult Problems()
        {
            var problems = problemReader.GetProblems();

            if (problems is null)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
            else
            {
                return Ok(problems);
            }
        }

        [HttpGet("problem/{problemId:int}")]
        public IActionResult Problem(int problemId)
        {
            var problem = problemReader.GetProblem(problemId);

            if (problem is null)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }
            else
            {
                return Ok(problem);
            }
        }

        [HttpPut("problem")]
        public IActionResult Problem([FromBody] CreateProblemDto problem)
        {
            if (ModelState.IsValid)
            {
                return Ok(problemCreator.CreateProblem(problem));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}