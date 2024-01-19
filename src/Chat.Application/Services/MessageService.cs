using Chat.Application.Contracts.Repositories;
using Chat.Application.Contracts.Services;
using Chat.Application.Exceptions;
using Chat.Application.Mappers;
using Chat.Common.Dtos;
using Chat.Domain.Entities;
using Chat.Infrastructure.Database;

namespace Chat.Application.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;
    private readonly IUserService _userService;
    private readonly IChannelService _channelService;
    private readonly ChatDataContext _dbContext;
    private readonly MessageMapper _messageMapper = new MessageMapper();

    public MessageService(
        IMessageRepository messageRepository,
        IUserService userService,
        IChannelService channelService,
        ChatDataContext dbContext
    )
    {
        _messageRepository = messageRepository;
        _userService = userService;
        _dbContext = dbContext;
        _channelService = channelService;
    }

    public Message WriteMessageToUser(MessageCreateDto messageCreateDto, string receiverId)
    {
        Message message = _messageMapper.MessageCreateDtoToMessage(messageCreateDto);
        User receiver = _userService.GetUserById(receiverId);
        string authorId = _userService.GetAuthenticatedUserId();
        User author = _userService.GetUserById(authorId);

        var transaction = _dbContext.Database.BeginTransaction();

        message.UserMessages.Add(new UserMessages() { Message = message, Receiver = receiver });
        message.Author = author;

        try
        {
            _messageRepository.Insert(message);
            _messageRepository.Save();
            transaction.Commit();
        }
        catch (Exception e)
        {
            transaction.Rollback();
            throw;
        }

        return message;
    }

    public Message WriteMessageToChannel(
        MessageCreateDto messageCreateDto,
        string serverId,
        string channelId
    )
    {
        Message message = _messageMapper.MessageCreateDtoToMessage(messageCreateDto);
        Channel receivingChannel = _channelService.GetChannelById(serverId, channelId);
        string authorId = _userService.GetAuthenticatedUserId();
        User author = _userService.GetUserById(authorId);

        var transaction = _dbContext.Database.BeginTransaction();

        message.ChannelMessages.Add(
            new ChannelMessage() { Message = message, Channel = receivingChannel }
        );
        message.Author = author;

        try
        {
            _messageRepository.Insert(message);
            _messageRepository.Save();
            transaction.Commit();
        }
        catch (Exception e)
        {
            transaction.Rollback();
            throw;
        }

        return message;
    }

    public ICollection<Message> GetUserMessages(string userId)
    {
        var messages = _messageRepository.GetUserMessages(userId);
        return messages;
    }

    public ICollection<Message> GetChannelMessages(string serverId, string userId)
    {
        var messages = _messageRepository.GetChannelMessages(userId);
        return messages;
    }

    public Message GetMessageById(string messageId)
    {
        var message = _messageRepository.GetById(messageId);

        if (message == null)
        {
            throw new BadRequestException("MessageDoesNotExist");
        }

        return message;
    }

    public Message UpdateUserMessage(
        MessageUpdateDto messageUpdateDto,
        string receiverId,
        string messageId
    )
    {
        _userService.GetUserById(receiverId);
        Message message = this.GetMessageById(messageId);
        var transaction = _dbContext.Database.BeginTransaction();

        message.Content = messageUpdateDto.Content;

        try
        {
            _messageRepository.Save();
            transaction.Commit();
        }
        catch (Exception e)
        {
            transaction.Rollback();
            throw;
        }

        return message;
    }

    public Message UpdateChannelMessage(
        MessageUpdateDto messageUpdateDto,
        string serverId,
        string channelId,
        string messageId
    )
    {
        _channelService.GetChannelById(serverId, channelId);
        Message message = this.GetMessageById(messageId);
        var transaction = _dbContext.Database.BeginTransaction();

        message.Content = messageUpdateDto.Content;

        try
        {
            _messageRepository.Save();
            transaction.Commit();
        }
        catch (Exception e)
        {
            transaction.Rollback();
            throw;
        }

        return message;
    }

    public void DeleteUserMessage(string receiverId, string messageId)
    {
        _userService.GetUserById(receiverId);
        Message message = this.GetMessageById(messageId);
        var transaction = _dbContext.Database.BeginTransaction();

        try
        {
            _messageRepository.SoftDelete(messageId);
            _messageRepository.Save();
            transaction.Commit();
        }
        catch (Exception e)
        {
            transaction.Rollback();
            throw;
        }
    }

    public void DeleteChannelMessage(string serverId, string channelId, string messageId)
    {
        _channelService.GetChannelById(serverId, channelId);
        Message message = this.GetMessageById(messageId);
        var transaction = _dbContext.Database.BeginTransaction();

        try
        {
            _messageRepository.SoftDelete(messageId);
            _messageRepository.Save();
            transaction.Commit();
        }
        catch (Exception e)
        {
            transaction.Rollback();
            throw;
        }
    }
}
