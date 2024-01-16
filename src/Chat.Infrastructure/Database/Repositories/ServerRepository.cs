using Chat.Application.Contracts.Repositories;
using Chat.Common.Dtos;
using Chat.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Database.Repositories;

public class ServerRepository : GenericRepository<Server>, IServerRepository
{
    public ServerRepository(ChatDataContext chatDataContext)
        : base(chatDataContext) { }

    public new Server? GetById(string id)
    {
        return _context
            .Server.Include(s => s.UserServers)
            .ThenInclude(s => s.User)
            .Include(s => s.Channels)
            .FirstOrDefault(s => s.Id == id);
    }
}
