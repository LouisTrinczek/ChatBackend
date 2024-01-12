using Chat.Common.Dtos;
using Chat.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Chat.Application.Mappers;

[Mapper]
public partial class UserMapper
{
    public partial User UserRegistrationDtoToUser(UserRegistrationDto userRegistrationDto);

    public partial User UserLoginDtoToUser(UserLoginDto userLoginDto);

    public partial User UserUpdateDtoToUser(UserUpdateDto userUpdateDto);

    public partial UserResponseDto UserToUserResponseDto(User user);
}
