using ChatApp.Backend.Core.Enums;

namespace ChatApp.Backend.Domain;

public class Conversation
{
    public int Id { get; set; }

    public ConversationType Type { get; set; }
    public List<Message> Messages { get; set; } = [];
    public List<ConversationUsers> GroupUsers { get; } = [];
    public List<User> Users { get; } = [];
}
