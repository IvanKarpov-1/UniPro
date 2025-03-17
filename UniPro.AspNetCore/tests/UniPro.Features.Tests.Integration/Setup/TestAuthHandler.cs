using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace UniPro.Features.Tests.Integration.Setup;

public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }

    public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new List<Claim>();

        if (Request.Headers.TryGetValue("X-Test-Claims", out var claimsJson))
        {
            var claimsDict = JsonSerializer.Deserialize<Dictionary<string, string>>(claimsJson!);
            if (claimsDict != null)
            {
                claims.AddRange(claimsDict.Select(kvp => new Claim(kvp.Key, kvp.Value)));
            }
        }
        
        var identity = new ClaimsIdentity(claims, "Bearer");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "TestScheme");
        
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}