using Chat.Common.Dtos;
using Chat.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Chat.Domain.Mappers;

[Mapper]
public partial class MessageMapper
{
    public partial MessageResponseDto MessageToMessageResponseDto(Message message);

    public partial Message MessageCreateDtoToMessage(MessageCreateDto messageCreateDto);

    public partial Message MessageUpdateDtoToMessage(MessageUpdateDto messageUpdateDto);
}
