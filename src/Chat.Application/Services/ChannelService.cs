using Chat.Application.Contracts.Repositories;
using Chat.Application.Contracts.Services;
using Chat.Application.Exceptions;
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
    private readonly IServerService _serverService;

    public ChannelService(
        IChannelRepository channelRepository,
        ChatDataContext context,
        Lazy<IServerService> serverService
    )
    {
        _channelRepository = channelRepository;
        _context = context;
        _serverService = serverService.Value;
    }

    public Channel Create(ServerChannelCreateDto serverChannelCreateDto, string serverId)
    {
        var transaction = _context.Database.BeginTransaction();
        var server = _serverService.GetServerById(serverId);

        _serverService.CheckIfAuthenticatedUserIsOwner(server);

        var channel = _channelMapper.ServerChannelCreateDtoToChannel(serverChannelCreateDto);

        channel.Server = server;

        try
        {
            _channelRepository.Insert(channel);
            _channelRepository.Save();
            transaction.Commit();
        }
        catch (Exception e)
        {
            transaction.Rollback();
            throw;
        }

        return channel;
    }

    public Channel Update(
        ServerChannelUpdateDto serverChannelUpdateDto,
        string serverId,
        string channelId
    )
    {
        var transaction = _context.Database.BeginTransaction();
        var updatedChannel = this.GetChannelById(serverId, channelId);

        _serverService.CheckIfAuthenticatedUserIsOwner(updatedChannel.Server);

        updatedChannel.Name = serverChannelUpdateDto.Name;

        try
        {
            _channelRepository.Save();
            transaction.Commit();
        }
        catch (Exception e)
        {
            transaction.Rollback();
            throw;
        }

        return updatedChannel;
    }

    public void Delete(string serverId, string channelId)
    {
        var transaction = _context.Database.BeginTransaction();
        var channelToDelete = this.GetChannelById(serverId, channelId);

        _serverService.CheckIfAuthenticatedUserIsOwner(channelToDelete.Server);

        try
        {
            _channelRepository.SoftDelete(channelId);
            _channelRepository.Save();
            transaction.Commit();
        }
        catch (Exception e)
        {
            transaction.Rollback();
            throw;
        }
    }

    public Channel GetChannelById(string serverId, string channelId)
    {
        this.CheckIfChannelIsPartOfServer(serverId, channelId);
        Channel? channel = _channelRepository.GetById(channelId);

        if (channel == null || channel.DeletedAt is not null)
        {
            throw new BadRequestException("ChannelDoesNotExist");
        }

        return channel;
    }

    public ICollection<Channel> GetAllChannelsOfServer(string serverId)
    {
        var server = _serverService.GetServerById(serverId);

        return server.Channels;
    }

    public void CheckIfChannelIsPartOfServer(string serverId, string channelId)
    {
        Server server = _serverService.GetServerById(serverId);

        var channelIsPartOfServer = server.Channels.Select(c => c.Id == channelId).FirstOrDefault();

        if (!channelIsPartOfServer)
        {
            throw new BadRequestException("ChannelIsNotPartOfServer");
        }
    }
}
