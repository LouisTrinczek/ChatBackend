using Chat.Application.Contracts.Repositories;
using Chat.Application.Contracts.Services;
using Chat.Common.Dtos;
using Microsoft.Extensions.Logging;

namespace Chat.Application.Services;

public class ServerService : IServerService
{
    private readonly IServerRepository _serverRepository;
    private readonly IUserService _userService;

    /// <summary>
    /// Dependency Injection
    /// </summary>
    public ServerService(IServerRepository serverRepository, IUserService userService)
    {
        _serverRepository = serverRepository;
        _userService = userService;
    }

    public ServerResponseDto Create(ServerCreationDto serverCreationDto)
    {
        throw new NotImplementedException();
    }

    public ServerResponseDto Update(ServerUpdateDto serverUpdateDto, string serverId)
    {
        throw new NotImplementedException();
    }

    public void Delete(string serverId)
    {
        throw new NotImplementedException();
    }
}
