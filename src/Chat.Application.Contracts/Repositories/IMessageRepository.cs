using Chat.Domain.Entities;

namespace Chat.Application.Contracts.Repositories;

public interface IMessageRepository : IGenericRepository<Message>
{
    public List<Message> GetUserMessages(string userId);
    public List<Message> GetChannelMessages(string userId);
}
