using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chat.Domain.Entities;

public class User : BaseEntity
{
    [Column] [MaxLength(320)] public string Email { get; set; } = null!;
    [Column] [MaxLength(100)] public string Username { get; set; } = null!;
    [Column] [MaxLength(100)] public string Password { get; set; } = null!;
    [Column] [MaxLength(100)] public string AvatarUrl { get; set; } = null!;
    public ICollection<Server> Servers { get; } = null!;
}