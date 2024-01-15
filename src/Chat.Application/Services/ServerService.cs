using Chat.Application.Contracts.Repositories;
using Chat.Application.Contracts.Services;
using Chat.Application.Exceptions;
using Chat.Application.Mappers;
using Chat.Common.Dtos;
using Chat.Domain.Entities;
using Chat.Infrastructure.Database;
using Microsoft.Extensions.Logging;

namespace Chat.Application.Services;

public class ServerService : IServerService
{
    private readonly ServerMapper _serverMapper = new ServerMapper();
    private readonly IServerRepository _serverRepository;
    private readonly IUserService _userService;
    private readonly IChannelService _channelService;
    private readonly ChatDataContext _dataContext;

    /// <summary>
    /// Dependency Injection
    /// </summary>
    public ServerService(
        IServerRepository serverRepository,
        IUserService userService,
        IChannelService channelService,
        ChatDataContext context
    )
    {
        _serverRepository = serverRepository;
        _userService = userService;
        _channelService = channelService;
        _dataContext = context;
    }

    public ServerResponseDto Create(ServerCreationDto serverCreationDto)
    {
        var serverToCreate = _serverMapper.ServerCreationDtoToServer(serverCreationDto);
        var userId = _userService.GetAuthenticatedUserId()!;
        var owner = _userService.GetUserById(userId);

        var transaction = _dataContext.Database.BeginTransaction();

        try
        {
            serverToCreate.Owner = owner;
            serverToCreate.Channels.Add(new Channel() { Name = "chat" });
            serverToCreate.UserServers.Add(
                new UserServer() { Server = serverToCreate, User = owner }
            );

            _serverRepository.Insert(serverToCreate);

            _serverRepository.Save();
        }
        catch (Exception e)
        {
            transaction.Rollback();
            throw;
        }

        return _serverMapper.ServerToServerResponseDto(serverToCreate);
    }

    public ServerResponseDto Update(ServerUpdateDto serverUpdateDto, string serverId)
    {
        throw new NotImplementedException();
    }

    public Server GetServerById(string serverId)
    {
        var server = _serverRepository.GetById(serverId);

        if (server == null)
        {
            throw new BadRequestException("ServerDoesNotExist");
        }

        return server;
    }

    public void Delete(string serverId)
    {
        _serverRepository.SoftDelete(serverId);
    }
}
