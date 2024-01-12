using System.ComponentModel.DataAnnotations.Schema;

namespace Chat.Domain.Entities;

public class Channel : BaseEntity
{
    [Column]
    public string Name { get; set; } = null!;
    public ICollection<Message> Messages = null!;
}
