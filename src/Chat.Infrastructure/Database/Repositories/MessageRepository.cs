using Chat.Application.Contracts.Repositories;
using Chat.Common.Dtos;
using Chat.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Database.Repositories;

public class MessageRepository : GenericRepository<Message>, IMessageRepository
{
    public MessageRepository(ChatDataContext chatDataContext)
        : base(chatDataContext) { }

    public List<Message> GetUserMessages(string userId)
    {
        var messages = _context
            .Users.Where(u => u.Id == userId)
            .Include(u => u.UserMessages)
            .ThenInclude(u => u.Message)
            .SelectMany(u => u.UserMessages.Select(um => um.Message))
            .Include(m => m.Author)
            .ToList();

        return messages;
    }

    public List<Message> GetChannelMessages(string channelId)
    {
        var messages = _context
            .Channel.Where(c => c.Id == channelId)
            .Include(u => u.ChannelMessages)
            .ThenInclude(u => u.Message)
            .SelectMany(u => u.ChannelMessages.Select(um => um.Message))
            .Include(m => m.Author)
            .ToList();

        return messages;
    }
}
