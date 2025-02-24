using ChatApp.Backend.Core.Enums;
using ChatApp.Backend.Core.Messages.DTOs;
using ChatApp.Backend.Core.Users.DTOs;
using ChatApp.Backend.Domain;

namespace ChatApp.Backend.Core.Conversations.DTOs;

public record ConversationListDto(
    int Id,
    ConversationType Type,
    List<UserDto> Members,
    LastMessageDto? LastMessage
);
