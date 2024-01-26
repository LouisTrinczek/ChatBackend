using Chat.Application.Contracts.Repositories;
using Chat.Common.Dtos;
using Chat.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Database.Repositories;

public class FriendRepository : GenericRepository<Friends>, IFriendRepository
{
    public FriendRepository(ChatDataContext chatDataContext)
        : base(chatDataContext) { }

    public List<Friends> GetFriendsList(string userId)
    {
        throw new NotImplementedException();
    }
}
