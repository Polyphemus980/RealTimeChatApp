using ChatApp.Backend.Core.Common;
using ChatApp.Backend.Core.Conversations.DTOs;
using ChatApp.Backend.Domain;

namespace ChatApp.Backend.Core.Conversations;

public interface IConversationService
{
    public Task<Result<List<ConversationListDto>>> GetUserConversations(string userId);

    public Task<Result<SingleConversationDto>> GetConversation(int conversationId, string userId);
    public Task<Result<Unit>> AddUserToGroup(int groupId, string userId);
    public Task<Result<Unit>> RemoveUserFromGroup(int groupId, string userId);

    public Task<Result<Unit>> ChangeNickname(int groupId, string userId, string? newNickname);

    public Task<Result<bool>> CheckIfUserInConversation(int conversationId, string userId);
}
