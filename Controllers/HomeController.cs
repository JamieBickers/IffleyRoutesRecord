using Microsoft.AspNetCore.Mvc;
using Logic;

namespace Iffley.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var holds = ClimbsAnalysis.getHoldsWithOccuranceRateOrderedAscending();
            return View();
        }
    }
}
