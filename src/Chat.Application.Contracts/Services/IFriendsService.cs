using Chat.Common.Dtos;
using Chat.Domain.Entities;

namespace Chat.Application.Contracts.Services;

public interface IFriendsService
{
    public User AddFriend(string senderId, string receiverId);
    public void RemoveFriend(string friendId);
    public ICollection<User> GetFriendList();
}
