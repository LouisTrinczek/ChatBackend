using System.Net.Mime;
using Chat.API.Services;
using Chat.Common.Dtos;
using Chat.Common.Types;
using Chat.Domain.Entities;
using Chat.Persistence.Context;
using Chat.Persistence.Repositories;
using EntityFramework.Exceptions.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BC = BCrypt.Net.BCrypt;

namespace Chat.API.Controllers;

/// <summary>
/// Provides the REST Endpoints for the User Endpoints such as Login, Registering and user editing
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("/api/v{version:apiVersion}/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class UsersController : ControllerBase
{
    private UserRepository _userRepository;
    private UserService _userService = new UserService();

    /// <summary>
    /// Dependency Injection
    /// </summary>
    public UsersController(ChatDataContext chatDataContext)
    {
        _userRepository = new UserRepository(chatDataContext);
    }

    /// <summary>Creates a new User</summary>
    /// <response code='200'>Successfully generated User</response>
    /// <response code='400'>Invalid Email or password too Short Password</response>
    /// <response code='409'>User with Email or Username already exists</response>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [Consumes(typeof(UserRegistrationDto), MediaTypeNames.Application.Json)]
    [Produces(typeof(ApiResponse<string?>))]
    public IActionResult Register([FromBody] UserRegistrationDto userRegistrationDto)
    {
        try
        {
            _userService.Register(userRegistrationDto);
            return Ok();
        }
        catch (Exception e)
        {
            // TODO: Add Exception Error Message
            Console.WriteLine(e);
            return BadRequest();
        }
    }

    /// <summary> Authenticates the User with a JWT Token </summary>
    /// <response code='200'>Successfully generated JWT Token String</response>
    /// <response code='401'>Wrong Email, Username or Password</response>
    /// <response code='400'>Invalid Email or too Short Password</response>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Consumes(typeof(UserLoginDto), MediaTypeNames.Application.Json)]
    [Produces(typeof(ApiResponse<string>))]
    public string Login([FromBody] UserLoginDto userLoginDto)
    {
        return "Not Implemented";
    }

    /// <summary>Updates a User</summary>
    /// <response code='200'>Successfully Updated User</response>
    /// <response code='401'>If the user isn't logged in</response>
    /// <response code='403'>If the user tries to update a user he's not permitted to</response>
    [HttpPut("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Consumes(typeof(UserUpdateDto), MediaTypeNames.Application.Json)]
    [Produces(typeof(ApiResponse<UserResponseDto>))]
    [Authorize]
    public string Update([FromBody] UserUpdateDto userUpdateDto)
    {
        return "Not Implemented";
    }

    /// <summary>Deletes a User</summary>
    /// <response code='200'>Successfully Deleted User</response>
    /// <response code='401'>If the user isn't logged in</response>
    /// <response code='403'>If the user tries to delete a user he's not permitted to</response>
    [HttpDelete("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Produces(typeof(ApiResponse<string>))]
    [Authorize]
    public string Delete()
    {
        return "Not Implemented";
    }

    /// <summary>Gets a User</summary>
    /// <response code='200'>Successfully Get User</response>
    /// <response code='401'>If the user isn't logged in</response>
    /// <response code='403'>If the user tries to get a user he's not permitted to</response>
    [HttpGet("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Produces(typeof(ApiResponse<UserResponseDto[]>))]
    [Authorize]
    public string Get()
    {
        return "String";
    }

    /// <summary>Writes a Message to a User</summary>
    /// <response code='200'>Successfully sent message</response>
    /// <response code='401'>If the user isn't logged in</response>
    /// <response code='403'>If the user tries to write a user he's not permitted to</response>
    [HttpPost("{userId}/messages")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ApiExplorerSettings(GroupName = "User Messages")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Produces(typeof(ApiResponse<MessageResponseDto>))]
    [Consumes(typeof(MessageCreateDto), MediaTypeNames.Application.Json)]
    [Authorize]
    public string WriteMessage([FromBody] MessageCreateDto messageCreateDto)
    {
        return "String";
    }

    /// <summary>Updates a Message</summary>
    /// <response code='200'>Successfully updated message</response>
    /// <response code='401'>If the user isn't logged in</response>
    /// <response code='403'>If the user tries to update a message he's not permitted to</response>
    [HttpPatch("{userId}/messages/{messageId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ApiExplorerSettings(GroupName = "User Messages")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Produces(typeof(ApiResponse<MessageResponseDto>))]
    [Consumes(typeof(MessageUpdateDto), MediaTypeNames.Application.Json)]
    [Authorize]
    public string UpdateMessage([FromBody] MessageUpdateDto messageUpdateDto)
    {
        return "String";
    }

    /// <summary>Deletes A Message</summary>
    /// <response code='200'>Successfully deleted message</response>
    /// <response code='401'>If the user isn't logged in</response>
    /// <response code='403'>If the user tries to delete a message he's not permitted to</response>
    [HttpDelete("{userId}/messages/{messageId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ApiExplorerSettings(GroupName = "User Messages")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Produces(typeof(ApiResponse<string>))]
    [Authorize]
    public string DeleteMessage()
    {
        return "String";
    }

    /// <summary>Gets a Paginated Chat with a user</summary>
    /// <response code='200'>Successfully get chat</response>
    /// <response code='401'>If the user isn't logged in</response>
    /// <response code='403'>If the user tries to get a chat he's not permitted to</response>
    [HttpGet("{userId}/messages")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ApiExplorerSettings(GroupName = "User Messages")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Produces(typeof(PaginatedApiResponse<MessageResponseDto[]>))]
    [Authorize]
    public string GetMessages()
    {
        return "String";
    }

    /// <summary>Gets all Servers a User is a member of</summary>
    /// <response code='200'>Successfully get servers</response>
    /// <response code='401'>If the user isn't logged in</response>
    /// <response code='403'>If the user tries to get a chat he's not permitted to</response>
    [HttpGet("{userId}/servers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ApiExplorerSettings(GroupName = "Users")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Produces(typeof(PaginatedApiResponse<MessageResponseDto[]>))]
    [Authorize]
    public string GetAllUserServers()
    {
        return "String";
    }
}
