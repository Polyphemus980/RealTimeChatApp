using System.Security.Claims;

namespace ChatApp.Backend.Core.Common;

public static class HttpContextExtensions
{
    public static string? GetAuthorizedUserId(this HttpContext context)
    {
        return context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    public static string? GetAuthorizedUserEmail(this HttpContext context)
    {
        return context.User.FindFirst(ClaimTypes.Email)?.Value;
    }
}
