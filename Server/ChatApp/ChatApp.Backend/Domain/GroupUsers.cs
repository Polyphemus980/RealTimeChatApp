using System.ComponentModel.DataAnnotations;

namespace ChatApp.Backend.Domain;

public class GroupUsers
{
    public string UserId { get; set; } = null!;
    public User User { get; set; } = null!;

    public int GroupId { get; set; }
    public Group Group { get; set; } = null!;

    [Required]
    public string Nickname { get; set; } = null!;

    public bool IsAdmin { get; set; }
}
