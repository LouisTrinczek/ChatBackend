using Chat.Common.Dtos;
using Chat.Domain.Entities;

namespace Chat.Application.Contracts.Services;

public interface IServerService
{
    public Server Create(ServerCreationDto serverCreationDto);
    public Server Update(ServerUpdateDto serverUpdateDto, string serverId);

    public Server GetServerById(string serverId);
    public void Delete(string serverId);
    public ICollection<Server> GetAllServersUserIsMemberOf(string userId);
    public void CheckIfAuthenticatedUserIsOwner(Server server);
}
