using System.Security.Claims;
using ChatApp.Backend.Core.Common;
using ChatApp.Backend.Core.Conversations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Backend.Api.Controllers;

[Authorize]
[ApiController]
[Route("conversations")]
public class ConversationController : ControllerBase
{
    private readonly IConversationService _conversationService;

    public ConversationController(IConversationService conversationService)
    {
        _conversationService = conversationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserConversations()
    {
        var userId = HttpContext.GetAuthorizedUserId()!;
        var result = await _conversationService.GetUserConversations(userId);
        return result.IsSuccess
            ? Ok(result.Data)
            : StatusCode(result.StatusCode, new { error = result.ErrorMessage });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetConversation(int id)
    {
        var userId = HttpContext.GetAuthorizedUserId()!;
        var canAccessConversationResult = await _conversationService.CheckIfUserInConversation(
            id,
            userId
        );
        if (canAccessConversationResult.IsSuccess)
        {
            return Unauthorized(canAccessConversationResult.ErrorMessage);
        }

        var result = await _conversationService.GetConversation(id, userId);
        return result.IsSuccess
            ? Ok(result.Data)
            : StatusCode(result.StatusCode, new { error = result.ErrorMessage });
    }
}
