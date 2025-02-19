using System.Security.Claims;
using ChatApp.Backend.Core.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ChatApp.Backend.Api.Controllers;

[Authorize]
[Route("user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("check-name")]
    public async Task<IActionResult> CheckIfNameFree([FromQuery] string name)
    {
        if (name.IsNullOrEmpty())
        {
            return BadRequest("Name parameter must be given");
        }
        var result = await _userService.IsNameFree(name);
        return result.IsSuccess
            ? Ok(new { isFree = result.Data })
            : StatusCode(
                StatusCodes.Status500InternalServerError,
                new { error = result.ErrorMessage }
            );
    }

    [HttpGet("check-new")]
    public async Task<IActionResult> CheckIfNewUser()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var result = await _userService.IsNewUser(userId);
        return result.IsSuccess
            ? Ok(new { isNew = result.Data })
            : StatusCode(result.StatusCode, new { error = result.ErrorMessage });
    }
}
