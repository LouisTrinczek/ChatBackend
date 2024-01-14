using Chat.Common.Dtos;
using Chat.Domain.Entities;

namespace Chat.Application.Contracts.Services;

public interface IChannelService
{
    public Channel Create(ServerChannelCreateDto serverChannelCreateDto, Server server);
}
