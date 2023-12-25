using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chat.Domain.Entities;

public class Channel : BaseEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Column]
    public string Name { get; set; } = null!;

    public ICollection<Message> Messages = null!;
}