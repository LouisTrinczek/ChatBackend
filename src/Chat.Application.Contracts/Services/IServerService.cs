using Chat.Common.Dtos;

namespace Chat.Application.Contracts.Services;

public interface IServerService
{
    public ServerResponseDto Create(ServerCreationDto serverCreationDto);
    public ServerResponseDto Update(ServerUpdateDto serverUpdateDto, string serverId);
    public void Delete(string serverId);
}
