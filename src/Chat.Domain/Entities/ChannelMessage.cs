namespace Chat.Domain.Entities;

public class ChannelMessage : BaseEntity
{
    public string ChannelId { get; set; } = null!;
    public string MessageId { get; set; } = null!;
    public Channel Channel { get; set; } = null!;
    public Message Message { get; set; } = null!;
}
