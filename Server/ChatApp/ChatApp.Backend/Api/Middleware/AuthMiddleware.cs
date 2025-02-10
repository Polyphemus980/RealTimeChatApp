using System.Security.Claims;
using Azure.Core;
using ChatApp.Backend.Core.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace ChatApp.Backend.Api.Middleware;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IAuthService _authService;

    public AuthMiddleware(RequestDelegate next, IAuthService authService)
    {
        _authService = authService;
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var path = context.Request.Path.Value?.ToLower();

        if (path == "/auth/verify")
        {
            await _next(context);
            return;
        }

        var authHeader = context.Request.Headers.Authorization;
        var token = authHeader.FirstOrDefault()?.Replace("Bearer ", "");
        var result = await _authService.VerifyTokenAsync(token);
        if (!result.IsSuccess)
        {
            context.Response.StatusCode = result.StatusCode;
            await context.Response.WriteAsync(result.ErrorMessage!);
            return;
        }

        var verifiedToken = result.Data!;
        var uid = verifiedToken.Uid!;
        var email = verifiedToken.Claims.TryGetValue("email", out var claim)
            ? claim?.ToString()
            : null;

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, uid),
            new Claim(ClaimTypes.Email, email ?? string.Empty),
        };

        var identity = new ClaimsIdentity(claims, "firebase");
        var principal = new ClaimsPrincipal(identity);

        context.User = principal;

        await _next(context);
    }
}
