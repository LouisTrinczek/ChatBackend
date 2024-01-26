using Chat.Common.Dtos;
using Chat.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Chat.Application.Mappers;

[Mapper]
public partial class ServerMapper
{
    [UseMapper]
    private readonly UserMapper _userMapper = new();

    [MapProperty(nameof(Server.UserServers), nameof(ServerResponseDto.Members))]
    public partial ServerResponseDto ServerToServerResponseDto(Server server);

    public partial ServerResponseDto[] ServerCollectionToServerResponseDtoList(
        ICollection<Server> server
    );

    public partial Server ServerCreationDtoToServer(ServerCreationDto serverCreationDto);

    public partial Server ServerUpdateDtoToServer(ServerUpdateDto serverUpdateDto);
}
