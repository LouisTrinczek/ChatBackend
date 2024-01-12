using Chat.Domain.Entities;

namespace Chat.Application.Contracts.Security;

public interface IJwtHandler
{
    public string GenerateToken(User user);
    public string? ValidateToken(string token);
}
