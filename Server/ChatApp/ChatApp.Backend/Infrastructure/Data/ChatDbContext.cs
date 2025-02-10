using ChatApp.Backend.Domain;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Backend.Infrastructure.Data;

public class ChatDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public ChatDbContext(DbContextOptions<ChatDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().Property(u => u.Email).IsRequired();
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

        modelBuilder.Entity<User>().Property(u => u.DisplayName).IsRequired().HasMaxLength(25);
    }
}
