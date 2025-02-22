﻿using System.Security.Claims;
using ChatApp.Backend.Core.Authentication;
using ChatApp.Backend.Domain;
using Microsoft.Extensions.Caching.Memory;

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
            new(ClaimTypes.NameIdentifier, uid),
            new(ClaimTypes.Email, email ?? string.Empty),
        };

        var identity = new ClaimsIdentity(claims, "firebase");
        var principal = new ClaimsPrincipal(identity);

        context.User = principal;

        await _next(context);
    }
}
