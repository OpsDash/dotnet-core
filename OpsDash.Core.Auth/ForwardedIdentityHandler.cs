using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace OpsDash.Core.Auth;

public class ForwardedIdentityHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder
) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(ForwardedIdentityDefaults.UserIdHeader, out var userIdValues))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var userId = userIdValues.ToString();
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Task.FromResult(AuthenticateResult.Fail("Missing user id header."));
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId),
            new("sub", userId)
        };

        if (Request.Headers.TryGetValue(ForwardedIdentityDefaults.UserNameHeader, out var name)
            && !string.IsNullOrWhiteSpace(name))
        {
            claims.Add(new(ClaimTypes.Name, name.ToString()));
        }

        if (Request.Headers.TryGetValue(ForwardedIdentityDefaults.EmailHeader, out var email)
            && !string.IsNullOrWhiteSpace(email))
        {
            claims.Add(new(ClaimTypes.Email, email.ToString()));
        }

        if (Request.Headers.TryGetValue(ForwardedIdentityDefaults.RolesHeader, out var roles)
            && !string.IsNullOrWhiteSpace(roles))
        {
            claims.AddRange(
                roles
                    .ToString()
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(role => new Claim(ClaimTypes.Role, role))
            );
        }

        var identity = new ClaimsIdentity(claims, ForwardedIdentityDefaults.AuthenticationScheme);
        var ticket = new AuthenticationTicket(new(identity), Scheme.Name);
        
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}