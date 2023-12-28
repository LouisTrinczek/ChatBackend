using Chat.Common.Dtos;
using Chat.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Chat.Domain.Mappers;

[Mapper]
public partial class ChannelMapper
{
    public partial ServerChannelResponseDto ServerChannelToChannelResponseDto(Channel channel);

    public partial Channel ServerChannelCreateDtoToChannel(
        ServerChannelCreateDto serverChannelCreateDto
    );

    public partial Channel ServerChannelUpdateDtoToChannel(
        ServerChannelUpdateDto serverChannelUpdateDto
    );
}
