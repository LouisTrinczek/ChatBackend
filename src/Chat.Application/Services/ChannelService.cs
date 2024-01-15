using Chat.Application.Contracts.Repositories;
using Chat.Application.Contracts.Services;
using Chat.Application.Mappers;
using Chat.Common.Dtos;
using Chat.Domain.Entities;
using Chat.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Chat.Application.Services;

public class ChannelService : IChannelService
{
    private readonly IChannelRepository _channelRepository;
    private readonly ChannelMapper _channelMapper = new ChannelMapper();
    private readonly ChatDataContext _context;

    public ChannelService(IChannelRepository channelRepository, ChatDataContext context)
    {
        _channelRepository = channelRepository;
        _context = context;
    }

    public Channel Create(ServerChannelCreateDto serverChannelCreateDto, Server server)
    {
        var channel = _channelMapper.ServerChannelCreateDtoToChannel(serverChannelCreateDto);
        var transaction = _context.Database.BeginTransaction();

        channel.Server = server;

        try
        {
            _channelRepository.Insert(channel);
            _channelRepository.Save();
        }
        catch (Exception e)
        {
            transaction.Rollback();
            throw;
        }

        return channel;
    }
}
