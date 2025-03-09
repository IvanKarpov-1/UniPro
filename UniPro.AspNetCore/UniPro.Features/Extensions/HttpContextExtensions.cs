using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace UniPro.Features.Extensions;

public static class HttpContextExtensions
{
    public static string? GetNameIdentifier(this HttpContext httpContext)
    {
        var user = httpContext.User;
        return user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? user.FindFirst("sub")?.Value;
    }
}