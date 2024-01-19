using Chat.Application.Contracts.Repositories;
using Chat.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Database.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(ChatDataContext chatDataContext)
        : base(chatDataContext) { }

    public new User? GetById(string userId)
    {
        return _context
            .Users.Where(u => u.Id == userId)
            .Include(u => u.UserServers)
            .ThenInclude(u => u.Server)
            .FirstOrDefault();
    }

    public User? GetByUsername(string username)
    {
        return _context.Users.FirstOrDefault(w => w.Username == username);
    }

    public User? GetByEmail(string email)
    {
        return _context.Users.FirstOrDefault(w => w.Email == email);
    }

    public User? GetByEmailOrUsername(string username, string email)
    {
        return _context.Users.FirstOrDefault(w => w.Email == email || w.Username == username);
    }
}
