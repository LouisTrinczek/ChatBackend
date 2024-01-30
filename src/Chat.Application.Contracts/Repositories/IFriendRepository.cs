using Chat.Domain.Entities;

namespace Chat.Application.Contracts.Repositories;

public interface IFriendRepository : IGenericRepository<Friends>
{
    public List<User> GetFriendsList(string userId);
    public List<User> GetReceivedFriendRequests(string userId);
    public List<User> GetSentFriendRequests(string userId);
    public Friends? GetFriendsFromSenderAndReceiver(User sender, User receiver);
    public void Delete(Friends friends);
}
