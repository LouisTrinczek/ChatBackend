using Chat.Common.Dtos;

namespace Chat.Application.Contracts.Services;

public interface IMessageService
{
    public MessageResponseDto WriteMessageToUser(string userId);
    public MessageResponseDto WriteMessageToChannel(string channelId);
}
