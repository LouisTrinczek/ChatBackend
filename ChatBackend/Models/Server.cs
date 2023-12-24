using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatBackend.Models;

public class Server : BaseEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [Column]
    public string Name { get; set; } = null!;
    
    public User Owner = null!;
    
    public ICollection<Channel> Channels { get; set; } = null!;
}