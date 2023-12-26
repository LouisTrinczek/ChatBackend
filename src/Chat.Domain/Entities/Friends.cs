using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chat.Domain.Entities;

public class Friends : BaseEntity
{
    [Key] [MaxLength(100)] public string Id { get; set; } = Guid.NewGuid().ToString();
    [Column] public User User { get; set; } = null!;
    [Column] public User Friend { get; set; } = null!;
}