using Chat.Application.Contracts.Repositories;
using Chat.Common.Dtos;
using Chat.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Database.Repositories;

public class FriendRepository : GenericRepository<Friends>, IFriendRepository
{
    public FriendRepository(ChatDataContext chatDataContext)
        : base(chatDataContext) { }

    public List<User> GetFriendsList(string userId)
    {
        return _table
            .Include(f => f.Receiver)
            .Include(f => f.Sender)
            .Where(f => f.Sender.Id == userId || f.Receiver.Id == userId)
            .Where(f => f.Accepted == true)
            .Select(f => f.Sender.Id == userId ? f.Receiver : f.Sender)
            .ToList();
    }

    public List<User> GetReceivedFriendRequests(string userId)
    {
        return _table
            .Include(f => f.Receiver)
            .Include(f => f.Sender)
            .Where(f => f.Accepted == false)
            .Where(f => f.Receiver.Id == userId)
            .Select(f => f.Sender)
            .ToList();
    }

    public List<User> GetSentFriendRequests(string userId)
    {
        return _table
            .Include(f => f.Receiver)
            .Include(f => f.Sender)
            .Where(f => f.Sender.Id == userId)
            .Select(f => f.Receiver)
            .ToList();
    }

    public Friends? GetFriendsFromSenderAndReceiver(User sender, User receiver)
    {
        return _table
            .Include(f => f.Receiver)
            .Include(f => f.Sender)
            .FirstOrDefault(
                f =>
                    (f.Sender.Id == sender.Id && f.Receiver.Id == receiver.Id)
                    || (f.Sender.Id == receiver.Id && f.Receiver.Id == sender.Id)
            );
    }

    public void Delete(Friends friends)
    {
        _table.Remove(friends);
    }
}
