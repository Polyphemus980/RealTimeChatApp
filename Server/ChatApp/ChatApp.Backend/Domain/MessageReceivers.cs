using ChatApp.Backend.Core.Enums;

namespace ChatApp.Backend.Domain;

public class MessageReceivers
{
    public int MessageId { get; set; }
    public Message Message { get; set; } = null!;

    public string UserId { get; set; } = null!;
    public User User { get; set; } = null!;

    public DateTime SentAt { get; set; }

    public MessageStatus Status { get; set; }
}
