using System.Security.Claims;
using System.Text.Json;

namespace UniPro.Features.Tests.Integration.Setup;

public static class HttpClientExtensions
{
    public static HttpClient AddClaims(this HttpClient client, List<Claim> claims)
    {
        var claimsDict = claims.ToDictionary(c => c.Type, c => c.Value);
        var claimsJson = JsonSerializer.Serialize(claimsDict);

        client.DefaultRequestHeaders.Add("X-Test-Claims", claimsJson);
        return client;
    }

    public static HttpClient OmitAuthentication(this HttpClient client)
    {
        client.DefaultRequestHeaders.Add("X-Omit-Authentication", "true");
        return client;
    }

    public static HttpClient RemoveTestHeaders(this HttpClient client)
    {
        client.DefaultRequestHeaders.Remove("X-Test-Claims");
        client.DefaultRequestHeaders.Remove("X-Omit-Authentication");
        return client;
    }
}