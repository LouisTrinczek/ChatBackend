using Chat.Domain.Entities;
using Chat.Persistence.Context;

namespace Chat.Persistence.Repositories;

public class UserRepository : GenericRepository<User>
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
}
