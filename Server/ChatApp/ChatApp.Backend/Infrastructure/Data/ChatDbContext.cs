using ChatApp.Backend.Domain;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Backend.Infrastructure.Data;

public class ChatDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Conversation> Conversations { get; set; }
    public DbSet<ConversationUsers> ConversationUsers { get; set; }
    public DbSet<MessageReceivers> MessageReceivers { get; set; }

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
            .Entity<User>()
            .HasMany(u => u.Conversations)
            .WithMany(g => g.Users)
            .UsingEntity<ConversationUsers>();

        modelBuilder.Entity<ConversationUsers>().HasKey(cu => new { cu.ConversationId, cu.UserId });
        modelBuilder.Entity<ConversationUsers>().Property(g => g.Nickname).HasMaxLength(25);

        modelBuilder
            .Entity<Message>()
            .HasOne(m => m.Sender)
            .WithMany(u => u.SentMessages)
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder
            .Entity<Message>()
            .HasOne(m => m.Conversation)
            .WithMany(g => g.Messages)
            .HasForeignKey(m => m.ConversationId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        modelBuilder.Entity<MessageReceivers>().HasKey(mr => new { mr.MessageId, mr.UserId });

        modelBuilder
            .Entity<Message>()
            .HasMany(m => m.Receivers)
            .WithMany(u => u.ReceivedMessages)
            .UsingEntity<MessageReceivers>();
    }
}
