using Chat.Common.Dtos;
using Chat.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Chat.Application.Mappers;

[Mapper]
public partial class ServerMapper
{
    public partial ServerResponseDto ServerToServerResponseDto(Server server);

    public partial Server ServerCreationDtoToServer(ServerCreationDto serverCreationDto);

    public partial Server ServerUpdateDtoToServer(ServerUpdateDto serverUpdateDto);
}
