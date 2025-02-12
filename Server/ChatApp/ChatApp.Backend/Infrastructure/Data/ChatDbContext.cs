using ChatApp.Backend.Domain;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Backend.Infrastructure.Data;

public class ChatDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupUsers> GroupUsers { get; set; }

    public ChatDbContext(DbContextOptions<ChatDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("IX_UNIQUE_EMAIL");

        modelBuilder.Entity<User>().Property(u => u.DisplayName).HasMaxLength(25);
        modelBuilder
            .Entity<User>()
            .HasIndex(u => u.DisplayName)
            .IsUnique()
            .HasDatabaseName("IX_UNIQUE_NAME");

        modelBuilder
            .Entity<Message>()
            .HasOne(m => m.Sender)
            .WithMany(u => u.SentMessages)
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder
            .Entity<Message>()
            .HasOne(m => m.ReceiverUser)
            .WithMany(u => u.ReceivedMessages)
            .HasForeignKey(m => m.ReceiverUserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        modelBuilder
            .Entity<Message>()
            .HasOne(m => m.ReceiverGroup)
            .WithMany(g => g.Messages)
            .HasForeignKey(m => m.ReceiverGroupId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        modelBuilder
            .Entity<Message>()
            .ToTable(messageTable =>
                messageTable.HasCheckConstraint(
                    "CK_MESSAGE_RECEIVER",
                    "(ReceiverUserId IS NOT NULL AND ReceiverGroupId IS NULL) OR "
                        + "(ReceiverUserId IS NULL AND ReceiverGroupId IS NOT NULL)"
                )
            );

        modelBuilder
            .Entity<User>()
            .HasMany(u => u.Groups)
            .WithMany(g => g.Users)
            .UsingEntity<GroupUsers>();
    }
}
