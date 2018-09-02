using IffleyRoutesRecord.Logic.DTOs.Sent;
using IffleyRoutesRecord.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace IffleyRoutesRecord.Controllers
{
    [Route("styleSymbol")]
    [ApiController]
    public class StyleSymbolController : Controller
    {
        private readonly IStyleSymbolManager styleSymbolManager;

        public StyleSymbolController(IStyleSymbolManager styleSymbolManager)
        {
            this.styleSymbolManager = styleSymbolManager;
        }

        [HttpGet]
        public ActionResult<IEnumerable<StyleSymbolDto>> GetStyleSymbols()
        {
            return styleSymbolManager.GetStyleSymbols().ToList();
        }

        [HttpGet("{styleSymbolId}")]
        public ActionResult<StyleSymbolDto> GetStyleSymbol(int styleSymbolId)
        {
            return styleSymbolManager.GetStyleSymbol(styleSymbolId);
        }
    }
}