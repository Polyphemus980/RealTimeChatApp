using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using ChatApp.Backend.Core.Authentication;
using ChatApp.Backend.Core.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ChatApp.Backend.Api.Controllers;

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

    [HttpPost("verify")]
    public async Task<IActionResult> VerifyToken([FromBody] string token)
    {
        var result = await _authService.VerifyTokenAsync(token);
        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, new { error = result.ErrorMessage! });

        var userId = result.Data!.Uid;
        var isNew = await _userService.IsNewUser(userId);
        return Ok(new { userId, isNew });
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromForm] RegisterForm form)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        var result = await _userService.CreateUserAsync(userId, email, form.DisplayName);
        return result.IsSuccess
            ? Ok(new { userId = result.Data! })
            : StatusCode(result.StatusCode, new { error = result.ErrorMessage! });
    }
}

public record RegisterForm([Required] string DisplayName);
