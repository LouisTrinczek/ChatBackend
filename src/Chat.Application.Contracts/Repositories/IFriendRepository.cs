using Chat.Domain.Entities;

namespace Chat.Application.Contracts.Repositories;

public interface IFriendRepository : IGenericRepository<Friends>
{
    public List<Friends> GetFriendsList(string userId);
}
