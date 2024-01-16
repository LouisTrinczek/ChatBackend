using Chat.Common.Dtos;
using Chat.Domain.Entities;

namespace Chat.Application.Contracts.Services;

public interface IChannelService
{
    public Channel Create(ServerChannelCreateDto serverChannelCreateDto, string serverId);
    public Channel Update(ServerChannelUpdateDto serverChannelUpdateDto, string channelId);
    public void Delete(string channelId);
}
