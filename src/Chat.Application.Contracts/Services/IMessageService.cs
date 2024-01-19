using Chat.Common.Dtos;
using Chat.Domain.Entities;

namespace Chat.Application.Contracts.Services;

public interface IMessageService
{
    public Message WriteMessageToUser(MessageCreateDto messageCreateDto, string receiverId);
    public Message WriteMessageToChannel(
        MessageCreateDto messageCreateDto,
        string serverId,
        string channelId
    );

    public ICollection<Message> GetUserMessages(string userId);
    public ICollection<Message> GetChannelMessages(string serverId, string channelId);

    public Message GetMessageById(string messageId);

    public Message UpdateUserMessage(
        MessageUpdateDto messageUpdateDto,
        string receiverId,
        string messageId
    );
    public Message UpdateChannelMessage(
        MessageUpdateDto messageUpdateDto,
        string serverId,
        string channelId,
        string messageId
    );

    public void DeleteUserMessage(string receiverId, string messageId);
    public void DeleteChannelMessage(string serverId, string channelId, string messageId);
}
