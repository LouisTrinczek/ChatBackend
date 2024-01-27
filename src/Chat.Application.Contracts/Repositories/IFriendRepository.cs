using Chat.Domain.Entities;

namespace Chat.Application.Contracts.Repositories;

public interface IFriendRepository : IGenericRepository<Friends>
{
    public List<Friends> GetFriendsList(string senderId, string friendId);
    public Friends? GetFriendsFromSenderAndReceiver(User sender, User receiver);
    public void Delete(Friends friends);
}
