using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chat.Domain.Entities;

public class Friends : BaseEntity
{
    [Column] public User User { get; set; } = null!;
    [Column] public User Friend { get; set; } = null!;
}