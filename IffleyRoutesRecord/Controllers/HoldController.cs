using System.Collections.Generic;
using System.Linq;
using IffleyRoutesRecord.Logic.DTOs.Responses;
using IffleyRoutesRecord.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IffleyRoutesRecord.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HoldController : ControllerBase
    {
        private readonly IHoldManager holdManager;

        public HoldController(IHoldManager holdManager)
        {
            this.holdManager = holdManager;
        }

        [HttpGet]
        [ResponseCache(Duration = 60 * 60 * 24)]
        public ActionResult<IEnumerable<HoldResponse>> GetHolds()
        {
            return holdManager.GetHolds().ToList();
        }

        [HttpGet("{holdId}")]
        [ResponseCache(Duration = 60 * 60 * 24)]
        public ActionResult<HoldResponse> GetHold(int holdId)
        {
            return holdManager.GetHold(holdId);
        }
    }
}
