using Chat.Application.Contracts.Repositories;
using Chat.Domain.Entities;

namespace Chat.Infrastructure.Database.Repositories;

public class ServerRepository : GenericRepository<Server>, IServerRepository
{
    public ServerRepository(ChatDataContext chatDataContext)
        : base(chatDataContext) { }
}
