﻿namespace ChatApp.Backend.Domain;

public class User
{
    public string Id { get; set; }

    public string Email { get; set; }

    public string DisplayName { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
