using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IffleyRoutesRecord.Logic.DTOs.Responses;
using IffleyRoutesRecord.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IffleyRoutesRecord.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RuleController : ControllerBase
    {
        private readonly IRuleManager ruleManager;

        public RuleController(IRuleManager ruleManager)
        {
            this.ruleManager = ruleManager;
        }

        [HttpGet("problem/{ruleId}")]
        public ActionResult<ProblemRuleResponse> GetProblemRule(int ruleId)
        {
            return ruleManager.GetProblemRule(ruleId);
        }

        [HttpGet("problem")]
        public ActionResult<IEnumerable<ProblemRuleResponse>> GetProblemRules()
        {
            return ruleManager.GetAllProblemRules().ToList();
        }

        [HttpGet("hold/{ruleId}")]
        public ActionResult<HoldRuleResponse> GetHoldRule(int ruleId)
        {
            return ruleManager.GetHoldRule(ruleId);
        }

        [HttpGet("hold")]
        public ActionResult<IEnumerable<HoldRuleResponse>> GetHoldRules()
        {
            return ruleManager.GetAllHoldRules().ToList();
        }
    }
}
