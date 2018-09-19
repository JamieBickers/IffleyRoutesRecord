#if DEBUG
#pragma warning disable CA1822
using Microsoft.AspNetCore.Mvc;
using System;

namespace IffleyRoutesRecord.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : Controller
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