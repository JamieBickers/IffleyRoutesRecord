#pragma warning disable CA1822
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;

namespace IffleyRoutesRecord.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class DevToolsController : BaseController
    {
        public DevToolsController(IConfiguration configuration) : base(configuration)
        {

        }
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