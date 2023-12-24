using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatBackend.Models;

public class Channel : BaseEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Column]
    public string Name { get; set; } = null!;

    public ICollection<Message> Messages = null!;
}