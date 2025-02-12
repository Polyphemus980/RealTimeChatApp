namespace ChatApp.Backend.Domain;

public class Group
{
    public int Id { get; set; }

    public List<Message> Messages { get; } = [];
    public List<GroupUsers> GroupUsers { get; } = [];
    public List<User> Users { get; } = [];
}
