using System.Data;
using Chat.API.Exceptions;
using Chat.Application.Contracts.Repositories;
using Chat.Application.Contracts.Security;
using Chat.Application.Contracts.Services;
using Chat.Application.Mappers;
using Chat.Common.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using BC = BCrypt.Net.BCrypt;

namespace Chat.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly UserMapper _userMapper = new UserMapper();
    private readonly IConfiguration _configuration;
    private readonly IJwtHandler _jwtHandler;

    public UserService(
        IUserRepository userRepository,
        IJwtHandler jwtHandler,
        IConfiguration configuration
    )
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _jwtHandler = jwtHandler;
    }

    /// <summary>
    /// Registration Logic. Generates a user with an encrypted password.
    /// </summary>
    /// <param name="userRegistrationDto"></param>
    /// <returns cref="ActionResult"></returns>
    /// <exception cref="CustomException"></exception>
    public void Register(UserRegistrationDto userRegistrationDto)
    {
        List<string> errors = new List<string>();

        var user = _userMapper.UserRegistrationDtoToUser(userRegistrationDto);

        if (userRegistrationDto.Password.Length < 7)
        {
            throw new InvalidDataException("PasswordTooShort");
        }

        user.Password = BC.HashPassword(userRegistrationDto.Password);

        if (!user.EmailIsValid())
        {
            throw new InvalidDataException("EmailIsNotValid");
        }

        if (
            _userRepository.GetByEmail(user.Email) != null
            || _userRepository.GetByUsername(user.Username) != null
        )
        {
            throw new DuplicateNameException("UserAlreadyExists");
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
    }

    public string Login(UserLoginDto userLoginDto)
    {
        var user = _userRepository.GetByEmailOrUsername(
            username: userLoginDto.Username!,
            email: userLoginDto.Email!
        );

        if (
            string.IsNullOrWhiteSpace(userLoginDto.Username)
            && string.IsNullOrEmpty(userLoginDto.Email)
        )
        {
            throw new InvalidDataException("LoginMustContainEmailOrUsername");
        }

        if (string.IsNullOrWhiteSpace(userLoginDto.Password))
        {
            throw new InvalidDataException("PasswordIsRequired");
        }

        if (user == null)
        {
            throw new UnauthorizedAccessException("UserDoesNotExist");
        }

        bool passwordIsCorrect = BC.Verify(userLoginDto.Password, user.Password);

        if (!passwordIsCorrect)
        {
            throw new UnauthorizedAccessException("EmailOrPasswordIsWrong");
        }

        var createdToken = _jwtHandler.GenerateToken(user);

        return createdToken;
    }

    public void Delete(int userId)
    {
        _userRepository.SoftDelete(userId);
    }
}
