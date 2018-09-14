#if DEBUG
using Microsoft.AspNetCore.Mvc;
using System;

namespace IffleyRoutesRecord.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("shutdown")]
        public IActionResult Shutdown()
        {
            Environment.Exit(0);
            throw new InvalidOperationException();
        }
    }
}
#endif