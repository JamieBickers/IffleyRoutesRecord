using IffleyRoutesRecord.Logic.DTOs.Responses;
using IffleyRoutesRecord.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace IffleyRoutesRecord.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StyleSymbolController : Controller
    {
        private readonly IStyleSymbolManager styleSymbolManager;

        public StyleSymbolController(IStyleSymbolManager styleSymbolManager)
        {
            this.styleSymbolManager = styleSymbolManager;
        }

        [HttpGet]
        [ResponseCache(Duration = 60 * 60 * 24)]
        public ActionResult<IEnumerable<StyleSymbolResponse>> GetStyleSymbols()
        {
            return styleSymbolManager.GetStyleSymbols().ToList();
        }

        [HttpGet("{styleSymbolId}")]
        [ResponseCache(Duration = 60 * 60 * 24)]
        public ActionResult<StyleSymbolResponse> GetStyleSymbol(int styleSymbolId)
        {
            return styleSymbolManager.GetStyleSymbol(styleSymbolId);
        }
    }
}