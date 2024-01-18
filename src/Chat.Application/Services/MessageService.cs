using Chat.Application.Contracts.Services;
using Chat.Common.Dtos;

namespace Chat.Application.Services;

public class MessageService : IMessageService
{
    public MessageResponseDto WriteMessageToUser(string userId)
    {
        throw new NotImplementedException();
    }

    public MessageResponseDto WriteMessageToChannel(string channelId)
    {
        throw new NotImplementedException();
    }
}
