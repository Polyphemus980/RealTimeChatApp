using ChatApp.Backend.Core.Enums;
using ChatApp.Backend.Core.Messages.DTOs;
using ChatApp.Backend.Core.Users.DTOs;

namespace ChatApp.Backend.Core.Conversations.DTOs;

public record SingleConversationDto(
    int Id,
    ConversationType Type,
    List<UserDto> Members,
    UserDto CurrentUser,
    List<ConversationMessageDto> Messages
);
