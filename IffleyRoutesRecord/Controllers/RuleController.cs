using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IffleyRoutesRecord.Logic.DTOs.Sent;
using IffleyRoutesRecord.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IffleyRoutesRecord.Controllers
{
    [Route("rule")]
    [ApiController]
    public class RuleController : ControllerBase
    {
        private readonly IRuleManager ruleManager;

        public RuleController(IRuleManager ruleManager)
        {
            this.ruleManager = ruleManager;
        }

        [HttpGet("problem/{ruleId}")]
        public ActionResult<ProblemRuleDto> GetProblemRule(int ruleId)
        {
            return ruleManager.GetProblemRule(ruleId);
        }

        [HttpGet("problem")]
        public ActionResult<IEnumerable<ProblemRuleDto>> GetProblemRules()
        {
            return ruleManager.GetAllProblemRules().ToList();
        }

        [HttpGet("hold/{ruleId}")]
        public ActionResult<HoldRuleDto> GetHoldRule(int ruleId)
        {
            return ruleManager.GetHoldRule(ruleId);
        }

        [HttpGet("hold")]
        public ActionResult<IEnumerable<HoldRuleDto>> GetHoldRules()
        {
            return ruleManager.GetAllHoldRules().ToList();
        }
    }
}
