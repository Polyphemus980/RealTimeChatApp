using System.ComponentModel.DataAnnotations;

namespace ChatApp.Backend.Domain;

public class ConversationUsers
{
    public string UserId { get; set; } = null!;
    public User User { get; set; } = null!;

    public int ConversationId { get; set; }
    public Conversation Conversation { get; set; } = null!;

    public string? Nickname { get; set; }

    public bool? IsAdmin { get; set; }
}
