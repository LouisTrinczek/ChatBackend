using Chat.Common.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Chat.API.Services;

public interface IUserService
{
    public ActionResult Register(UserRegistrationDto userRegistrationDto);
}
