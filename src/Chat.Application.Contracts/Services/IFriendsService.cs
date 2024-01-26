using Chat.Common.Dtos;
using Chat.Domain.Entities;

namespace Chat.Application.Contracts.Services;

public interface IFriendsService
{
    public User AddFriend(FriendsRequestDto friendsRequestDto);
    public void RemoveFriend(string friendId);
    public ICollection<User> GetFriendList();
}
