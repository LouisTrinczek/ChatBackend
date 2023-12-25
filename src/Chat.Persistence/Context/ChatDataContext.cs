using Chat.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chat.Persistence.Context;

public class ChatDataContext : DbContext
{
    public ChatDataContext(DbContextOptions<ChatDataContext> dbContextOptions) : base(dbContextOptions)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Server> Server { get; set; } = null!;
    public DbSet<Channel> Channel { get; set; } = null!;
    public DbSet<Message> Message { get; set; } = null!;
}