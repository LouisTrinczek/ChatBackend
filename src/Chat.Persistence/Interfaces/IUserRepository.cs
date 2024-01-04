using Chat.Domain;
using Chat.Domain.Entities;

namespace Chat.Persistence.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
   public User? GetByUsername(string username);
   public User? GetByEmail(string email);
   public User? GetByEmailOrUsername(string username, string email);
}