using Chat.Application.Contracts.Repositories;
using Chat.Domain.Entities;

namespace Chat.Infrastructure.Database.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(ChatDataContext chatDataContext)
        : base(chatDataContext) { }

    public User? GetByUsername(string username)
    {
        return _context.Users.SingleOrDefault(w => w.Username == username);
    }

    public User? GetByEmail(string email)
    {
        return _context.Users.SingleOrDefault(w => w.Email == email);
    }

    public User? GetByEmailOrUsername(string username, string email)
    {
        return _context.Users.SingleOrDefault(w => w.Email == email || w.Username == username);
    }
}
