using Chat.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Database;

public class ChatDataContext : DbContext
{
    public ChatDataContext(DbContextOptions<ChatDataContext> dbContextOptions)
        : base(dbContextOptions) { }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Server> Server { get; set; } = null!;
    public DbSet<Channel> Channel { get; set; } = null!;
    public DbSet<Message> Message { get; set; } = null!;
    public DbSet<Friends> Friends { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var builder = modelBuilder.Entity<User>();
        builder.HasIndex(c => c.Email).IsUnique();
        builder.HasIndex(c => c.Username).IsUnique();
    }
}
