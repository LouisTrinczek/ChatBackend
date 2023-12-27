using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chat.Domain.Entities;

public class Message : BaseEntity
{
    [Column] public string Content = null!;
    public User Author { get; set; } = null!;
    public Channel Channel { get; set; } = null!;
}