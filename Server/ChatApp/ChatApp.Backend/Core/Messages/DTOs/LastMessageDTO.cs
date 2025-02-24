namespace ChatApp.Backend.Core.Messages.DTOs;

public record LastMessageDto(int Id, string Content, DateTime SentAt, string SenderName);
