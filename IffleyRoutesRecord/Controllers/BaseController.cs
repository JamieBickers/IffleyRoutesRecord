using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace IffleyRoutesRecord.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly IConfiguration Configuration;

        protected BaseController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected string UserEmail => User?.Claims
            ?.SingleOrDefault(claim => claim.Type == $"{Configuration["Auth0:ApiIdentifier"]}/email")?.Value;
    }
}
