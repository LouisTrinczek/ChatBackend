using Chat.Common.Dtos;
using Chat.Domain.Entities;

namespace Chat.Application.Contracts.Services;

public interface IUserService
{
    public UserResponseDto Register(UserRegistrationDto userRegistrationDto);
    public string Login(UserLoginDto userLoginDto);
    public User GetUserById(string userId);
    public UserResponseDto Update(UserUpdateDto userUpdateDto, string userId);
    public void Delete(string userId);
    public string GetAuthenticatedUserId();
}
