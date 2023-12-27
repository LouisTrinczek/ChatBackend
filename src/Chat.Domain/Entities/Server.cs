using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chat.Domain.Entities;

public class Server : BaseEntity
{
    [Column] public string Name { get; set; } = null!;
    [Column] [MaxLength(100)] public string IconUrl { get; set; } = null!;
    public User Owner = null!;
    public ICollection<Channel> Channels { get; set; } = null!;
}