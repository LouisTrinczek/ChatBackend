﻿using System.Data;
using System.Security.Claims;
using Chat.Application.Contracts.Repositories;
using Chat.Application.Contracts.Security;
using Chat.Application.Contracts.Services;
using Chat.Application.Exceptions;
using Chat.Application.Mappers;
using Chat.Common.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using BC = BCrypt.Net.BCrypt;

namespace Chat.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly UserMapper _userMapper = new UserMapper();
    private readonly IConfiguration _configuration;
    private readonly IJwtHandler _jwtHandler;
    private readonly HttpContext _httpContext;

    public UserService(
        IUserRepository userRepository,
        IJwtHandler jwtHandler,
        IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _jwtHandler = jwtHandler;
        _httpContext = httpContextAccessor.HttpContext!;
    }

    /// <summary>
    /// Registration Logic. Generates a user with an encrypted password.
    /// </summary>
    /// <param name="userRegistrationDto"></param>
    /// <returns cref="ActionResult"></returns>
    /// <exception cref="CustomException"></exception>
    public void Register(UserRegistrationDto userRegistrationDto)
    {
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
            username: userLoginDto.Username ?? "",
            email: userLoginDto.Email ?? ""
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
            throw new BadRequestException("UserDoesNotExist");
        }

        bool passwordIsCorrect = BC.Verify(userLoginDto.Password, user.Password);

        if (!passwordIsCorrect)
        {
            throw new BadRequestException("EmailOrPasswordIsWrong");
        }

        var createdToken = _jwtHandler.GenerateToken(user);

        return createdToken;
    }

    public UserResponseDto Update(UserUpdateDto userUpdateDto, string userId)
    {
        var userToUpdate = _userRepository.GetById(userId);
        var mappedUser = _userMapper.UserUpdateDtoToUser(userUpdateDto);
        var authenticatedUserId = _jwtHandler.GetAuthenticatedClaimValue(ClaimTypes.NameIdentifier);

        if (userToUpdate == null || userToUpdate.DeletedAt is not null)
        {
            throw new BadRequestException("UserDoesNotExist");
        }

        if (authenticatedUserId != userToUpdate.Id)
        {
            throw new ForbiddenException($"NotPermittedToDeleteUser");
        }

        // Check if Email is taken except by myself
        var emailUser = _userRepository.GetByEmail(mappedUser.Email);
        if (emailUser is not null && emailUser.Id != authenticatedUserId)
        {
            throw new ConflictException("EmailIsAlreadyTaken");
        }

        // Check if Username is taken except by myself
        var usernameUser = _userRepository.GetByUsername(mappedUser.Username);
        if (usernameUser is not null && usernameUser.Id != authenticatedUserId)
        {
            throw new ConflictException("UsernameIsAlreadyTaken");
        }

        userToUpdate.Email = mappedUser.Email;
        userToUpdate.Username = mappedUser.Username;
        userToUpdate.Password = BC.HashPassword(mappedUser.Password);

        _userRepository.Save();

        return _userMapper.UserToUserResponseDto(userToUpdate);
    }

    public void Delete(string userId)
    {
        var userToDelete = _userRepository.GetById(userId);
        var authenticatedUserId = _jwtHandler.GetAuthenticatedClaimValue(ClaimTypes.NameIdentifier);

        if (userToDelete == null || userToDelete.DeletedAt is not null)
        {
            throw new BadRequestException("UserDoesNotExist");
        }

        if (authenticatedUserId != userToDelete.Id)
        {
            throw new ForbiddenException($"NotPermittedToDeleteUser");
        }

        if (userToDelete is null || userToDelete.DeletedAt != null)
        {
            throw new BadRequestException($"UserDoesNotExist");
        }

        _userRepository.SoftDelete(userId);
        _userRepository.Save();
    }
}
