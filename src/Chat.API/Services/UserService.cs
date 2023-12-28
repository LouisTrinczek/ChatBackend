using Chat.API.Exceptions;
using Chat.Common.Dtos;
using Chat.Common.Types;
using Chat.Domain.Entities;
using Chat.Persistence.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BC = BCrypt.Net.BCrypt;

namespace Chat.API.Services;

public class UserService
{
    // TODO: Figure out how to do DependencyInjection in Services to provide the Repositories via DependencyInjection and not via the Constructor or reinitializing a new class.
    private UserRepository _userRepository;

    public ActionResult Register(UserRegistrationDto userRegistrationDto)
    {
        List<string> errors = new List<string>();

        User user = new User();

        // TODO: Remove this Validation and use the DTO Validation if the Required values are set
        /*
        if (userRegistrationDto.Email == string.Empty)
        {
            throw new CustomException("EmailIsRequired");
        }

        if (userRegistrationDto.Username == string.Empty)
        {
            throw new CustomException("UsernameIsRequired");
        }

        if (userRegistrationDto.Password == string.Empty)
        {
            throw new CustomException("PasswordIsRequired");
        }
        */

        if (userRegistrationDto.Password.Length < 7)
        {
            throw new CustomException("PasswordTooShort");
        }

        user.Email = userRegistrationDto.Email;
        user.Username = userRegistrationDto.Username;
        user.Password = BC.HashPassword(userRegistrationDto.Password);

        /*
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
        */

        _userRepository.Insert(user);

        try
        {
            // _userRepository.Save();
        }
        catch (Exception e)
        {
            throw new CustomException("ErrorWhileCreatingUser");
        }

        return new OkResult();
    }
}
