namespace ChatApp.Backend.Core.Messages.DTOs;

public record ConversationMessageDto(
    int Id,
    List<MessageReceiverDTO> Receivers,
    string Content,
    string SenderId,
    DateTime SentAt
);
