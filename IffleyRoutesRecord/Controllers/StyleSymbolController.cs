using IffleyRoutesRecord.Logic.Interfaces;
using IffleyRoutesRecord.Models.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace IffleyRoutesRecord.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StyleSymbolController : Controller
    {
        private readonly IStyleSymbolManager styleSymbolManager;

        public StyleSymbolController(IStyleSymbolManager styleSymbolManager)
        {
            this.styleSymbolManager = styleSymbolManager;
        }

        /// <summary>
        /// Gets a list of all style symbols.
        /// </summary>
        /// <returns>The full list of holds</returns>
        /// <response code="200">The full list of holds</response>
        /// <response code="500">Unexpected error</response>
        [HttpGet]
        [ResponseCache(Duration = 60 * 60 * 24)]
        public ActionResult<IEnumerable<StyleSymbolResponse>> GetStyleSymbols()
        {
            return styleSymbolManager.GetStyleSymbols().ToList();
        }

        /// <summary>
        /// Gets a style symbol by its ID.
        /// </summary>
        /// <param name="styleSymbolId">The ID of the style symbol to return</param>
        /// <returns>The style symbol with the given ID</returns>
        /// <response code="200">The requested style symbol</response>
        /// <response code="404">No style symbol with the provided ID was found</response>
        /// <response code="500">Unexpected error</response>
        [HttpGet("{styleSymbolId}")]
        [ResponseCache(Duration = 60 * 60 * 24)]
        public ActionResult<StyleSymbolResponse> GetStyleSymbol(int styleSymbolId)
        {
            return styleSymbolManager.GetStyleSymbol(styleSymbolId);
        }
    }
}