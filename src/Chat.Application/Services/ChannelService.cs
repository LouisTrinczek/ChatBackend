using Chat.Application.Contracts.Repositories;
using Chat.Application.Contracts.Services;
using Chat.Application.Mappers;
using Chat.Common.Dtos;
using Chat.Domain.Entities;

namespace Chat.Application.Services;

public class ChannelService : IChannelService
{
    private readonly IChannelRepository _channelRepository;
    private readonly ChannelMapper _channelMapper = new ChannelMapper();

    public ChannelService(IChannelRepository channelRepository)
    {
        _channelRepository = channelRepository;
    }

    public Channel Create(ServerChannelCreateDto serverChannelCreateDto, Server server)
    {
        var channel = _channelMapper.ServerChannelCreateDtoToChannel(serverChannelCreateDto);

        channel.Server = server;

        _channelRepository.Insert(channel);
        _channelRepository.Save();

        return channel;
    }
}
