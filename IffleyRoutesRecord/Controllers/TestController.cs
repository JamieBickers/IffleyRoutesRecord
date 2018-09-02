#if DEBUG
using Microsoft.AspNetCore.Mvc;
using System;

namespace IffleyRoutesRecord.Controllers
{
    [Route("test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("shutdown")]
        public IActionResult Shutdown()
        {
            Environment.Exit(0);
            throw new Exception();
        }
    }
}
#endif