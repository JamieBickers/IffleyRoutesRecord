using System.Collections.Generic;
using System.Linq;
using IffleyRoutesRecord.Logic.DTOs.Sent;
using IffleyRoutesRecord.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IffleyRoutesRecord.Controllers
{
    [Route("hold")]
    [ApiController]
    public class HoldController : ControllerBase
    {
        private readonly IHoldManager holdManager;

        public HoldController(IHoldManager holdManager)
        {
            this.holdManager = holdManager;
        }

        [HttpGet]
        public ActionResult<IEnumerable<HoldDto>> GetHolds()
        {
            return holdManager.GetHolds().ToList();
        }

        [HttpGet("{holdId}")]
        public ActionResult<HoldDto> GetHold(int holdId)
        {
            return holdManager.GetHold(holdId);
        }
    }
}
