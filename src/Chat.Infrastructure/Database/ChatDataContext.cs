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
    public DbSet<Friends> UserFriends { get; set; } = null!;
    public DbSet<UserServers> UserServers { get; set; } = null!;
    public DbSet<UserMessages> UserMessages { get; set; } = null!;
    public DbSet<ChannelMessage> ChannelMessages { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ChatDataContext).Assembly);
    }
}
