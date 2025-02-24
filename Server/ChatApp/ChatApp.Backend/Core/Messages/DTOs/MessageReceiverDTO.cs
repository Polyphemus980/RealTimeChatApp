using ChatApp.Backend.Core.Enums;
using ChatApp.Backend.Core.Users.DTOs;

namespace ChatApp.Backend.Core.Messages.DTOs;

public record MessageReceiverDTO(string userId, MessageStatus status);
