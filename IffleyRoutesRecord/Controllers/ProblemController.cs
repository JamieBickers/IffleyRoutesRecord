using IffleyRoutesRecord.Logic.DTOs.Received;
using IffleyRoutesRecord.Logic.DTOs.Sent;
using IffleyRoutesRecord.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace IffleyRoutesRecord.Controllers
{
    [Route("problem")]
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
        public ActionResult<IEnumerable<ProblemDto>> GetProblems()
        {
            return problemReader.GetProblems().ToList();
        }

        [HttpGet("{problemId}")]
        public ActionResult<ProblemDto> GetProblem(int problemId)
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

        [HttpPut]
        public ActionResult<ProblemDto> CreateProblem(CreateProblemDto problem)
        {
            return problemCreator.CreateProblem(problem);
        }
    }
}