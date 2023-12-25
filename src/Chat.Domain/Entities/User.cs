using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chat.Domain.Entities;

public class User : BaseEntity
{
    [Key] [MaxLength(100)] public string Id { get; set; } = Guid.NewGuid().ToString();

    [Column] [MaxLength(100)] public string Username { get; set; } = null!;

    [Column] [MaxLength(100)] public string Password { get; set; } = null!;

    public ICollection<Server> Servers { get; } = null!;
}