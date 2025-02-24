using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using ChatApp.Backend.Core.Authentication;
using ChatApp.Backend.Core.Common;
using ChatApp.Backend.Core.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ChatApp.Backend.Api.Controllers;

[Authorize]
[Route("auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;

    public AuthController(IAuthService authService, IUserService userService)
    {
        _authService = authService;
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterForm form)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = HttpContext.GetAuthorizedUserId();
        var email = HttpContext.GetAuthorizedUserEmail();

        var result = await _userService.CreateUserAsync(userId, email, form.DisplayName);
        return result.IsSuccess
            ? Created()
            : StatusCode(result.StatusCode, new { error = result.ErrorMessage! });
    }
}

public record RegisterForm([Required] string DisplayName = null!);
