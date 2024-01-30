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

        var friend = _friendRepository.GetFriendsFromSenderAndReceiver(sender, receiver);

        if (friend is not null)
        {
            throw new ForbiddenException("UsersAreAlreadyFriends");
        }

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

    public void RemoveFriend(string senderId, string receiverId)
    {
        this.CheckIfCurrentUserIsSender(senderId);
        var transaction = _dbContext.Database.BeginTransaction();

        var friend = this.GetFriends(senderId, receiverId);

        try
        {
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

    public void AcceptFriendRequest(string userId, string friendId)
    {
        var friendship = this.GetFriends(userId, friendId);
        this.CheckIfCurrentUserIsReceiver(friendship.Receiver.Id);

        var transaction = _dbContext.Database.BeginTransaction();

        try
        {
            friendship.Accepted = true;
            _friendRepository.Save();
            transaction.Commit();
        }
        catch (Exception e)
        {
            transaction.Rollback();
            throw;
        }
    }

    public ICollection<User> GetFriendList(string userId)
    {
        this.CheckIfCurrentUserIsSender(userId);

        return _friendRepository.GetFriendsList(userId);
    }

    public ICollection<User> GetReceivedFriendRequestsList(string userId)
    {
        this.CheckIfCurrentUserIsSender(userId);

        return _friendRepository.GetReceivedFriendRequests(userId);
    }

    public ICollection<User> GetSentFriendRequestsList(string userId)
    {
        this.CheckIfCurrentUserIsSender(userId);

        return _friendRepository.GetSentFriendRequests(userId);
    }

    private void CheckIfCurrentUserIsSender(string senderId)
    {
        if (senderId != _userService.GetAuthenticatedUserId())
        {
            throw new ForbiddenException("UserIsNotFriendRequestSender");
        }
    }

    private void CheckIfCurrentUserIsReceiver(string receiverId)
    {
        if (receiverId != _userService.GetAuthenticatedUserId())
        {
            throw new ForbiddenException("UserIsNotFriendRequestReceiver");
        }
    }

    private Friends GetFriends(string senderId, string receiverId)
    {
        var sender = _userService.GetUserById(senderId);
        var receiver = _userService.GetUserById(receiverId);

        var friend = _friendRepository.GetFriendsFromSenderAndReceiver(sender, receiver);

        if (friend == null)
        {
            throw new ForbiddenException("UsersAreNotFriends");
        }

        return friend;
    }
}
