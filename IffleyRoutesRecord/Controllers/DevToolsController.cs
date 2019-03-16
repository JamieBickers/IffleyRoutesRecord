#pragma warning disable CA1822
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace IffleyRoutesRecord.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class DevToolsController : Controller
    {
        [HttpGet("shutdown")]
        public IActionResult Shutdown()
        {
#if DEBUG
            Environment.Exit(0);
#endif
            return StatusCode((int)HttpStatusCode.Forbidden);
        }
    }
}