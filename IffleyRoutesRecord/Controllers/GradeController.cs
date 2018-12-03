using System.Collections.Generic;
using System.Linq;
using IffleyRoutesRecord.Logic.Interfaces;
using IffleyRoutesRecord.Models.DTOs.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IffleyRoutesRecord.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradeController : Controller
    {
        private readonly IGradeManager gradeManager;

        public GradeController(IGradeManager gradeManager)
        {
            this.gradeManager = gradeManager;
        }

        /// <summary>
        /// Gets a list of all tech grades.
        /// </summary>
        /// <returns>The list of tech grades</returns>
        /// <response code="200">The list of tech grades</response>
        /// <response code="500">Unexpected error</response>
        [HttpGet("tech")]
        [ResponseCache(Duration = 60 * 60 * 24)]
        public ActionResult<IEnumerable<TechGradeResponse>> GetTechGrades()
        {
            return gradeManager.GetTechGrades().ToList();
        }

        /// <summary>
        /// Gets a tech grade by its ID.
        /// </summary>
        /// <param name="gradeId">The ID of the tech grade to return</param>
        /// <returns>The tech grade with the given ID</returns>
        /// <response code="200">The requested tech grade</response>
        /// <response code="404">No tech grade with the provided ID was found</response>
        /// <response code="500">Unexpected error</response>
        [HttpGet("tech/{gradeId}")]
        [ResponseCache(Duration = 60 * 60 * 24)]
        public ActionResult<TechGradeResponse> GetTechGrade(int gradeId)
        {
            return gradeManager.GetTechGrade(gradeId);
        }

        /// <summary>
        /// Gets a list of all B grades.
        /// </summary>
        /// <returns>The list of B grades</returns>
        /// <response code="200">The list of B grades</response>
        /// <response code="500">Unexpected error</response>
        [HttpGet("b")]
        [ResponseCache(Duration = 60 * 60 * 24)]
        public ActionResult<IEnumerable<BGradeResponse>> GetBGrades()
        {
            return gradeManager.GetBGrades().ToList();
        }

        /// <summary>
        /// Gets a B grade by its ID.
        /// </summary>
        /// <param name="gradeId">The ID of the B grade to return</param>
        /// <returns>The B grade with the given ID</returns>
        /// <response code="200">The requested B grade</response>
        /// <response code="404">No B grade with the provided ID was found</response>
        /// <response code="500">Unexpected error</response>
        [HttpGet("b/{gradeId}")]
        [ResponseCache(Duration = 60 * 60 * 24)]
        public ActionResult<BGradeResponse> GetBGrade(int gradeId)
        {
            return gradeManager.GetBGrade(gradeId);
        }

        /// <summary>
        /// Gets a list of all Povey grades.
        /// </summary>
        /// <returns>The list of Povey grades</returns>
        /// <response code="200">The list of Povey grades</response>
        /// <response code="500">Unexpected error</response>
        [HttpGet("povey")]
        [ResponseCache(Duration = 60 * 60 * 24)]
        public ActionResult<IEnumerable<PoveyGradeResponse>> GetPoveyGrades()
        {
            return gradeManager.GetPoveyGrades().ToList();
        }

        /// <summary>
        /// Gets a Povey grade by its ID.
        /// </summary>
        /// <param name="gradeId">The ID of the Povey grade to return</param>
        /// <returns>The Povey grade with the given ID</returns>
        /// <response code="200">The requested Povey grade</response>
        /// <response code="404">No Povey grade with the provided ID was found</response>
        /// <response code="500">Unexpected error</response>
        [HttpGet("povey/{gradeId}")]
        [ResponseCache(Duration = 60 * 60 * 24)]
        public ActionResult<PoveyGradeResponse> GetPoveyGrade(int gradeId)
        {
            return gradeManager.GetPoveyGrade(gradeId);
        }

        /// <summary>
        /// Gets a list of all Furlong grades.
        /// </summary>
        /// <returns>The list of Furlong grades</returns>
        /// <response code="200">The list of Furlong grades</response>
        /// <response code="500">Unexpected error</response>
        [HttpGet("furlong")]
        [ResponseCache(Duration = 60 * 60 * 24)]
        public ActionResult<IEnumerable<FurlongGradeResponse>> GetFurlongGrades()
        {
            return gradeManager.GetFurlongGrades().ToList();
        }

        /// <summary>
        /// Gets a Furlong grade by its ID.
        /// </summary>
        /// <param name="gradeId">The ID of the Furlong grade to return</param>
        /// <returns>The Furlong grade with the given ID</returns>
        /// <response code="200">The requested Furlong grade</response>
        /// <response code="404">No Furlong grade with the provided ID was found</response>
        /// <response code="500">Unexpected error</response>
        [HttpGet("furlong/{gradeId}")]
        [ResponseCache(Duration = 60 * 60 * 24)]
        public ActionResult<FurlongGradeResponse> GetFurlongGrade(int gradeId)
        {
            return gradeManager.GetFurlongGrade(gradeId);
        }
    }
}
