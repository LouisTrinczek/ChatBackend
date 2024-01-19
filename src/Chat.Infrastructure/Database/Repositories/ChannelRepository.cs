using Chat.Application.Contracts.Repositories;
using Chat.Common.Dtos;
using Chat.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Database.Repositories;

public class ChannelRepository : GenericRepository<Channel>, IChannelRepository
{
    public ChannelRepository(ChatDataContext chatDataContext)
        : base(chatDataContext) { }

    public new Channel? GetById(string channelId)
    {
        return _context
            .Channel.Where(c => c.Id == channelId)
            .Include(c => c.Server)
            .ThenInclude(c => c.Owner)
            .FirstOrDefault(c => c.Id == channelId);
    }
}
