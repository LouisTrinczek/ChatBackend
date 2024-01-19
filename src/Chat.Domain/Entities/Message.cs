using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chat.Domain.Entities;

public class Message : BaseEntity
{
    [Column]
    public string Content { get; set; } = null!;
    public User Author { get; set; } = null!;
    public ICollection<UserMessages> UserMessages { get; set; } = new List<UserMessages>();
    public ICollection<ChannelMessage> ChannelMessages { get; set; } = new List<ChannelMessage>();
}
