using Chat.Common.Dtos;
using Chat.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Chat.Application.Mappers;

[Mapper]
public partial class MessageMapper
{
    public partial MessageResponseDto MessageToMessageResponseDto(Message message);

    public partial MessageResponseDto[] MessageCollectionToMessageResponseDtoArray(
        ICollection<Message> message
    );

    public partial Message MessageCreateDtoToMessage(MessageCreateDto messageCreateDto);

    public partial Message MessageUpdateDtoToMessage(MessageUpdateDto messageUpdateDto);

    public ICollection<MessageResponseDto> ChannelMessagesToMessageResponseDtos(
        ICollection<ChannelMessage> channelMessages
    )
    {
        return channelMessages.Select(cm => this.MessageToMessageResponseDto(cm.Message)).ToList();
    }
}
