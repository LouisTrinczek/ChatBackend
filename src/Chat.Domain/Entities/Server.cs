using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Chat.Domain.Entities;

public class Server : BaseEntity
{
    [Column]
    public string Name { get; set; } = null!;

    [Column]
    [MaxLength(100)]
    public string? IconUrl { get; set; } = null!;

    public User Owner { get; set; } = null!;
    public ICollection<Channel> Channels { get; set; } = new List<Channel>();
    public ICollection<UserServers> UserServers { get; set; } = new List<UserServers>();
}
