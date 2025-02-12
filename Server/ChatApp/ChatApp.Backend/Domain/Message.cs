using System.ComponentModel.DataAnnotations;

namespace ChatApp.Backend.Domain;

public enum MessageStatus
{
    Sent,
    Delivered,
    Read,
}

public class Message
{
    public int Id { get; set; }

    public User Sender { get; set; }
    public string SenderId { get; set; }

    public Group? ReceiverGroup { get; set; }
    public int? ReceiverGroupId { get; set; }

    public User? ReceiverUser { get; set; }
    public string? ReceiverUserId { get; set; }

    public DateTime CreatedAt { get; set; }
    public MessageStatus Status { get; set; }

    // For now only text content
    [Required]
    public string Content { get; set; }
}
