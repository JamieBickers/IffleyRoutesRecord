using System;
using Microsoft.AspNetCore.Authorization;

namespace IffleyRoutesRecord.Auth
{
    internal class HasScopeRequirement : IAuthorizationRequirement
    {
        internal string Issuer { get; }
        internal string Scope { get; }

        internal HasScopeRequirement(string scope, string issuer)
        {
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
            Issuer = issuer ?? throw new ArgumentNullException(nameof(issuer));
        }
    }
}
