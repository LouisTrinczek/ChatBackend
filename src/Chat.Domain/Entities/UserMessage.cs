namespace Chat.Domain.Entities;

public class UserMessage : BaseEntity
{
    public string ReceiverId { get; set; } = null!;
    public string MessageId { get; set; } = null!;
    public User Receiver { get; set; } = null!;
    public Message Message { get; set; } = null!;
}
