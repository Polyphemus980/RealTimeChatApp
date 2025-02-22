using System.ComponentModel.DataAnnotations;
using ChatApp.Backend.Core.Enums;

namespace ChatApp.Backend.Domain;

public class Message
{
    public int Id { get; set; }

    public User Sender { get; set; } = null!;
    public string SenderId { get; set; } = null!;

    public Conversation Conversation { get; set; } = null!;
    public int ConversationId { get; set; }

    public DateTime CreatedAt { get; set; }

    // For now only text content
    [Required]
    public string Content { get; set; } = null!;

    public List<MessageReceivers> MessageStatuses { get; } = [];

    public List<User> Receivers { get; } = [];
}
