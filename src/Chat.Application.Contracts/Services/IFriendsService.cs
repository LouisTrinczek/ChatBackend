using Chat.Common.Dtos;
using Chat.Domain.Entities;

namespace Chat.Application.Contracts.Services;

public interface IFriendsService
{
    public User AddFriend(string senderId, string receiverId);
    public void RemoveFriend(string senderId, string receiverId);
    public ICollection<User> GetFriendList();
    public ICollection<User> GetReceivedFriendRequestsList();
    public ICollection<User> GetSentFriendRequestsList();
}
