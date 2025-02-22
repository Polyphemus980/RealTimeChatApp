using System.ComponentModel.DataAnnotations;

namespace ChatApp.Backend.Domain;

public class User
{
    public string Id { get; set; } = null!;

    [Required]
    public string Email { get; set; } = null!;

    [Required]
    public string DisplayName { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public List<Message> SentMessages { get; } = [];

    public List<Message> ReceivedMessages { get; } = [];

    public List<ConversationUsers> UserConversations { get; } = [];

    public List<Conversation> Conversations { get; } = [];

    public List<MessageReceivers> MessageStatuses { get; } = [];
}
