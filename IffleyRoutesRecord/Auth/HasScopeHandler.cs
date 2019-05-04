using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace IffleyRoutesRecord.Auth
{
    [SuppressMessage("Microsoft.Performance", "CA1812")]
    internal class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasScopeRequirement requirement)
        {
            // If user does not have the scope claim, get out of here
            if (!context.User.HasClaim(claim => claim.Type == "scope" && claim.Issuer == requirement.Issuer))
            {
                return Task.CompletedTask;
            }

            // Split the scopes string into an array
            string[] scopes = context.User.FindFirst(claim => claim.Type == "scope" && claim.Issuer == requirement.Issuer).Value.Split(' ');
            string[] allowedScopes = requirement.Scope.Split(' ');

            // Succeed if the scope array contains the required scope
            if (scopes.Any(scope => allowedScopes.Contains(scope)))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
