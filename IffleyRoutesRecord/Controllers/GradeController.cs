using System.Collections.Generic;
using System.Linq;
using IffleyRoutesRecord.Logic.DTOs.Responses;
using IffleyRoutesRecord.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IffleyRoutesRecord.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GradeController : ControllerBase
    {
        private readonly IGradeManager gradeManager;

        public GradeController(IGradeManager gradeManager)
        {
            this.gradeManager = gradeManager;
        }

        [HttpGet("tech")]
        [ResponseCache(Duration = 60 * 60 * 24)]
        public ActionResult<IEnumerable<TechGradeResponse>> GetTechGrades()
        {
            return gradeManager.GetTechGrades().ToList();
        }

        [HttpGet("tech/{gradeId}")]
        [ResponseCache(Duration = 60 * 60 * 24)]
        public ActionResult<TechGradeResponse> GetTechGrade(int gradeId)
        {
            return gradeManager.GetTechGrade(gradeId);
        }

        [HttpGet("b")]
        [ResponseCache(Duration = 60 * 60 * 24)]
        public ActionResult<IEnumerable<BGradeResponse>> GetBGrades()
        {
            return gradeManager.GetBGrades().ToList();
        }

        [HttpGet("b/{gradeId}")]
        [ResponseCache(Duration = 60 * 60 * 24)]
        public ActionResult<BGradeResponse> GetBGrade(int gradeId)
        {
            return gradeManager.GetBGrade(gradeId);
        }

        [HttpGet("povey")]
        [ResponseCache(Duration = 60 * 60 * 24)]
        public ActionResult<IEnumerable<PoveyGradeResponse>> GetPoveyGrades()
        {
            return gradeManager.GetPoveyGrades().ToList();
        }

        [HttpGet("povey/{gradeId}")]
        [ResponseCache(Duration = 60 * 60 * 24)]
        public ActionResult<PoveyGradeResponse> GetPoveyGrade(int gradeId)
        {
            return gradeManager.GetPoveyGrade(gradeId);
        }

        [HttpGet("furlong")]
        [ResponseCache(Duration = 60 * 60 * 24)]
        public ActionResult<IEnumerable<FurlongGradeResponse>> GetFurlongGrades()
        {
            return gradeManager.GetFurlongGrades().ToList();
        }

        [HttpGet("furlong/{gradeId}")]
        [ResponseCache(Duration = 60 * 60 * 24)]
        public ActionResult<FurlongGradeResponse> GetFurlongGrade(int gradeId)
        {
            return gradeManager.GetFurlongGrade(gradeId);
        }
    }
}
