namespace Chat.Domain.Entities;

public class Friends : BaseEntity
{
    public string SenderId { get; set; } = null!;
    public string ReceiverId { get; set; } = null!;

    public User Sender { get; set; } = null!;
    public User Receiver { get; set; } = null!;

    public bool Accepted { get; set; } = false;
}
