using Chat.Common.Dtos;

namespace Chat.Application.Contracts.Services;

public interface IUserService
{
    public void Register(UserRegistrationDto userRegistrationDto);
    public string Login(UserLoginDto userLoginDto);
    public void Delete(string userId);
}
