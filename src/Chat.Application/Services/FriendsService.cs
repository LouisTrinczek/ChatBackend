using Chat.Application.Contracts.Repositories;
using Chat.Application.Contracts.Services;
using Chat.Application.Exceptions;
using Chat.Common.Dtos;
using Chat.Domain.Entities;
using Chat.Infrastructure.Database;
using Chat.Infrastructure.Database.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Chat.Application.Services;

public class FriendsService : IFriendsService
{
    private readonly IFriendRepository _friendRepository;
    private readonly IUserService _userService;
    private readonly ChatDataContext _dbContext;

    public FriendsService(
        IFriendRepository friendRepository,
        IUserService userService,
        ChatDataContext dbContext
    )
    {
        _friendRepository = friendRepository;
        _userService = userService;
        _dbContext = dbContext;
    }

    public User AddFriend(string senderId, string receiverId)
    {
        this.CheckIfCurrentUserIsSender(senderId);

        var sender = _userService.GetUserById(senderId);
        var receiver = _userService.GetUserById(receiverId);

        var transaction = _dbContext.Database.BeginTransaction();
        try
        {
            Friends friendRequest = new Friends() { Sender = sender, Receiver = receiver };

            _friendRepository.Insert(friendRequest);
            _friendRepository.Save();
            transaction.Commit();
        }
        catch (Exception e)
        {
            transaction.Rollback();
            throw;
        }

        return receiver;
    }

    public void RemoveFriend(string senderId, string friendId)
    {
        this.CheckIfCurrentUserIsSender(senderId);
        var transaction = _dbContext.Database.BeginTransaction();

        var sender = _userService.GetUserById(senderId);
        var receiver = _userService.GetUserById(friendId);

        var friend = _friendRepository.GetFriendsFromSenderAndReceiver(sender, receiver);

        try
        {
            if (friend == null)
            {
                throw new BadRequestException("UsersAreNotFriends");
            }
            _friendRepository.Delete(friend);
            _friendRepository.Save();
            transaction.Commit();
        }
        catch (Exception e)
        {
            transaction.Rollback();
            throw;
        }
    }

    public ICollection<User> GetFriendList()
    {
        throw new NotImplementedException();
    }

    public ICollection<User> GetReceivedFriendRequestsList()
    {
        throw new NotImplementedException();
    }

    public ICollection<User> GetSentFriendRequestsList()
    {
        throw new NotImplementedException();
    }

    private void CheckIfCurrentUserIsSender(string senderId)
    {
        if (senderId != _userService.GetAuthenticatedUserId())
        {
            throw new ForbiddenException("UserNotPermittedToSendFriendRequestForUser");
        }
    }
}
