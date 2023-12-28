using System.Runtime.CompilerServices;
using Chat.API.Exceptions;
using Chat.Common.Dtos;
using Chat.Common.Types;
using Chat.Domain.Entities;
using Chat.Domain.Mappers;
using Chat.Persistence.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BC = BCrypt.Net.BCrypt;

namespace Chat.API.Services;

public class UserService : IUserService
{
    private readonly UserRepository _userRepository;
    private readonly UserMapper _userMapper = new UserMapper();

    public UserService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /// <summary>
    /// Registration Logic. Generates a user with an encrypted password.
    /// </summary>
    /// <param name="userRegistrationDto"></param>
    /// <returns cref="ActionResult"></returns>
    /// <exception cref="CustomException"></exception>
    public ActionResult Register(UserRegistrationDto userRegistrationDto)
    {
        List<string> errors = new List<string>();

        var user = _userMapper.UserRegistrationDtoToUser(userRegistrationDto);

        if (userRegistrationDto.Password.Length < 7)
        {
            throw new CustomException("PasswordTooShort");
        }

        user.Password = BC.HashPassword(userRegistrationDto.Password);

        if (!user.EmailIsValid())
        {
            throw new CustomException("EmailIsNotValid");
        }

        if (
            _userRepository.GetByEmail(user.Email) != null
            || _userRepository.GetByUsername(user.Username) != null
        )
        {
            throw new CustomException("UserAlreadyExists");
        }

        _userRepository.Insert(user);

        try
        {
            _userRepository.Save();
        }
        catch (Exception e)
        {
            throw new CustomException("ErrorWhileCreatingUser");
        }

        return new OkResult();
    }
}
