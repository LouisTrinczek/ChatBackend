using System.Security.Authentication;
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

    public Server Create(ServerCreationDto serverCreationDto)
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
            transaction.Commit();
        }
        catch (Exception e)
        {
            transaction.Rollback();
            throw;
        }

        return serverToCreate;
    }

    public Server Update(ServerUpdateDto serverUpdateDto, string serverId)
    {
        var transaction = _dataContext.Database.BeginTransaction();

        var updatedServer = this.GetServerById(serverId);

        this.CheckIfAuthenticatedUserIsOwner(updatedServer);

        updatedServer.Name = serverUpdateDto.Name;

        try
        {
            _serverRepository.Save();
            transaction.Commit();
        }
        catch (Exception e)
        {
            transaction.Rollback();
            throw;
        }

        return updatedServer;
    }

    public Server GetServerById(string serverId)
    {
        var server = _serverRepository.GetById(serverId);
        var authenticatedUserId = _userService.GetAuthenticatedUserId();

        if (server == null)
        {
            throw new BadRequestException("ServerDoesNotExist");
        }

        var userIsMember = server
            .UserServers.Select(c => c.UserId == authenticatedUserId)
            .FirstOrDefault();

        if (!userIsMember)
        {
            throw new ForbiddenException("UserIsNotAServerMember");
        }

        return server;
    }

    public void Delete(string serverId)
    {
        var serverToDelete = this.GetServerById(serverId);
        var transaction = _dataContext.Database.BeginTransaction();

        this.CheckIfAuthenticatedUserIsOwner(serverToDelete);

        try
        {
            _serverRepository.SoftDelete(serverId);
            _serverRepository.Save();
            transaction.Commit();
        }
        catch (Exception e)
        {
            transaction.Rollback();
            throw;
        }
    }

    public void CheckIfAuthenticatedUserIsOwner(Server server)
    {
        var authenticatedUserId = _userService.GetAuthenticatedUserId();

        if (server.Owner.Id != authenticatedUserId)
        {
            throw new ForbiddenException("UserIsNotServerOwner");
        }
    }
}
