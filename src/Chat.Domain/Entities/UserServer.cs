namespace Chat.Domain.Entities;

public class UserServer : BaseEntity
{
    public string UserId { get; set; } = null!;
    public string ServerId { get; set; } = null!;
    public User User { get; set; } = null!;
    public Server Server { get; set; } = null!;
}
