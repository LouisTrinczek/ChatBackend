using ChatBackend.Models;

namespace ChatBackend.Data;

using Microsoft.EntityFrameworkCore;

public class ChatDataContext : DbContext
{
    // TODO: Add Configuration file or something
    private readonly string _connectionString = @"Server=localhost; Port=40001; User ID=root; Password=secret; Database=chat";

    public DbSet<User> Users { get; set; } = null!;
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString));
    }
}