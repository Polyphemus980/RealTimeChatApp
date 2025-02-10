using System.ComponentModel.DataAnnotations;
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

        var userId = result.Data!;
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

        var result = await _userService.CreateUserAsync(form.Email, form.DisplayName);
        return result.IsSuccess
            ? Ok(new { userId = result.Data! })
            : StatusCode(result.StatusCode, new { error = result.ErrorMessage! });
    }
}

public record RegisterForm([Required] [EmailAddress] string Email, [Required] string DisplayName);
