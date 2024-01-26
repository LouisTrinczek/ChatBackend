using Chat.Application.Contracts.Repositories;
using Chat.Application.Contracts.Services;
using Chat.Common.Dtos;
using Chat.Domain.Entities;
using Chat.Infrastructure.Database.Repositories;

namespace Chat.Application.Services;

public class FriendsService : IFriendsService
{
    private readonly IFriendRepository _friendRepository;

    public FriendsService(IFriendRepository friendRepository)
    {
        _friendRepository = friendRepository;
    }

    public User AddFriend(FriendsRequestDto friendsRequestDto)
    {
        throw new NotImplementedException();
    }

    public void RemoveFriend(string friendId)
    {
        throw new NotImplementedException();
    }

    public ICollection<User> GetFriendList()
    {
        throw new NotImplementedException();
    }
}
