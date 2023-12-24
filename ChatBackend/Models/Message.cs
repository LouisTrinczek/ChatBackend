using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatBackend.Models;

public class Message : BaseEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Column]
    public string Content = null!;
    
    public User Author { get; set; } = null!;

    public Channel Channel { get; set; } = null!;
}