using Chat.Common.Dtos;

namespace Chat.Application.Contracts.Services;

public interface IUserService
{
    public void Register(UserRegistrationDto userRegistrationDto);
    public string Login(UserLoginDto userLoginDto);
    public UserResponseDto Update(UserUpdateDto userUpdateDto, string userId);
    public void Delete(string userId);
}
