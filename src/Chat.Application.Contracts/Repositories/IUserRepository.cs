using Chat.Domain.Entities;

namespace Chat.Application.Contracts.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    public User? GetByUsername(string username);
    public User? GetByEmail(string email);
    public User? GetByEmailOrUsername(string username, string email);
}
