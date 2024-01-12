using Chat.Common.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Application.Services;

public interface IUserService
{
    public void Register(UserRegistrationDto userRegistrationDto);
    public string Login(UserLoginDto userLoginDto);
    public void Delete(int userId);
}
