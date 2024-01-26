using Chat.Common.Dtos;
using Chat.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Chat.Application.Mappers;

[Mapper]
public partial class ChannelMapper
{
    [UseMapper]
    private readonly MessageMapper _messageMapper = new();

    [MapProperty(nameof(Channel.ChannelMessages), nameof(ServerChannelResponseDto.Messages))]
    public partial ServerChannelResponseDto ServerChannelToChannelResponseDto(Channel channel);

    public partial ServerChannelResponseDto[] ServerChannelCollectionToChannelResponseDtoList(
        ICollection<Channel> channel
    );

    public partial Channel ServerChannelCreateDtoToChannel(
        ServerChannelCreateDto serverChannelCreateDto
    );

    public partial Channel ServerChannelUpdateDtoToChannel(
        ServerChannelUpdateDto serverChannelUpdateDto
    );
}
