using System.ComponentModel.DataAnnotations;

namespace Chat.Domain.Entities;

public class BaseEntity
{
    [Key] public string Id { get; set; } = Guid.NewGuid().ToString();
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public DateTime? DeletedAt { get; set; } = null;
}