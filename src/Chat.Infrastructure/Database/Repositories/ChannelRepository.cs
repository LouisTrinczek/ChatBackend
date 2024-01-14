using Chat.Application.Contracts.Repositories;
using Chat.Common.Dtos;
using Chat.Domain.Entities;

namespace Chat.Infrastructure.Database.Repositories;

public class ChannelRepository : GenericRepository<Channel>, IChannelRepository
{
    public ChannelRepository(ChatDataContext chatDataContext)
        : base(chatDataContext) { }
}
