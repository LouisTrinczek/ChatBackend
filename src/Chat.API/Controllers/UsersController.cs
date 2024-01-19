using System.Data;
using System.Net.Mime;
using Chat.Application.Contracts.Services;
using Chat.Application.Exceptions;
using Chat.Application.Mappers;
using Chat.Common.Dtos;
using Chat.Common.Types;
using Chat.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
    private readonly IUserService _userService;
    private readonly IServerService _serverService;
    private readonly IMessageService _messageService;
    private readonly ILogger<UsersController> _logger;
    private readonly UserMapper _userMapper = new UserMapper();
    private readonly MessageMapper _messageMapper = new MessageMapper();

    /// <summary>
    /// Dependency Injection
    /// </summary>
    public UsersController(
        IUserService userService,
        ILogger<UsersController> logger,
        IServerService serverService,
        IMessageService messageService
    )
    {
        _serverService = serverService;
        _userService = userService;
        _logger = logger;
        _messageService = messageService;
    }

    /// <summary>Creates a new User</summary>
    /// <response code='200'>Successfully generated User</response>
    /// <response code='400'>Invalid Email or password too Short</response>
    /// <response code='409'>User with Email or Username already exists</response>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [Consumes(typeof(UserRegistrationDto), MediaTypeNames.Application.Json)]
    [Produces(typeof(ApiResponse<UserResponseDto>))]
    public IActionResult Register([FromBody] UserRegistrationDto userRegistrationDto)
    {
        try
        {
            var userResponseDto = _userService.Register(userRegistrationDto);
            return Ok(
                new ApiResponse<UserResponseDto>(
                    ResponseStatus.Success,
                    userResponseDto,
                    new string[] { }
                )
            );
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            ObjectResult exception = e switch
            {
                DuplicateNameException
                    => Conflict(
                        new ApiResponse<object>(
                            ResponseStatus.Error,
                            null,
                            new string[] { e.Message }
                        )
                    ),
                InvalidDataException
                    => BadRequest(
                        new ApiResponse<object>(
                            ResponseStatus.Error,
                            null,
                            new string[] { e.Message }
                        )
                    ),
                _
                    => BadRequest(
                        new ApiResponse<object>(
                            ResponseStatus.Error,
                            null,
                            new string[] { "UnknownError" }
                        )
                    ),
            };

            return exception;
        }
    }

    /// <summary> Authenticates the User with a JWT Token </summary>
    /// <response code='200'>Successfully generated JWT Token String</response>
    /// <response code='400'>Invalid Email, wrong Password, or user does not exist</response>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Consumes(typeof(UserLoginDto), MediaTypeNames.Application.Json)]
    [Produces(typeof(ApiResponse<string>))]
    public IActionResult Login([FromBody] UserLoginDto userLoginDto)
    {
        try
        {
            var token = _userService.Login(userLoginDto);
            return Ok(new ApiResponse<string>(ResponseStatus.Success, token, new string[] { }));
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            ObjectResult exception = e switch
            {
                BadRequestException
                or InvalidDataException
                    => BadRequest(
                        new ApiResponse<object>(
                            ResponseStatus.Error,
                            null,
                            new string[] { e.Message }
                        )
                    ),
                _
                    => BadRequest(
                        new ApiResponse<object>(
                            ResponseStatus.Error,
                            null,
                            new string[] { "UnknownError" }
                        )
                    ),
            };

            return exception;
        }
    }

    /// <summary>Updates a User</summary>
    /// <response code='200'>Successfully Updated User</response>
    /// <response code='401'>If the user isn't logged in</response>
    /// <response code='403'>If the user tries to update a user he's not permitted to</response>
    /// <response code='409'>If the user tries to update to a username or email that is already taken</response>
    [HttpPut("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [Consumes(typeof(UserUpdateDto), MediaTypeNames.Application.Json)]
    [Produces(typeof(ApiResponse<UserResponseDto>))]
    [Authorize]
    public IActionResult Update([FromBody] UserUpdateDto userUpdateDto, [FromRoute] string userId)
    {
        try
        {
            var userResponseDto = _userService.Update(userUpdateDto, userId);
            return Ok(
                new ApiResponse<UserResponseDto>(
                    ResponseStatus.Success,
                    userResponseDto,
                    new string[] { }
                )
            );
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            ObjectResult exception = e switch
            {
                InvalidDataException
                    => BadRequest(
                        new ApiResponse<object>(
                            ResponseStatus.Error,
                            null,
                            new string[] { e.Message }
                        )
                    ),
                UnauthorizedAccessException
                    => Unauthorized(
                        new ApiResponse<object>(
                            ResponseStatus.Error,
                            null,
                            new string[] { e.Message }
                        )
                    ),
                ConflictException
                    => Conflict(
                        new ApiResponse<object>(
                            ResponseStatus.Error,
                            null,
                            new string[] { e.Message }
                        )
                    ),
                BadRequestException
                    => BadRequest(
                        new ApiResponse<object>(
                            ResponseStatus.Error,
                            null,
                            new string[] { e.Message }
                        )
                    ),
                ForbiddenException
                    => new Forbidden(
                        new ApiResponse<object>(
                            ResponseStatus.Error,
                            null,
                            new string[] { e.Message }
                        )
                    ),
                _
                    => BadRequest(
                        new ApiResponse<object>(
                            ResponseStatus.Error,
                            null,
                            new string[] { "UnknownError" }
                        )
                    ),
            };

            return exception;
        }
    }

    /// <summary>Deletes a User</summary>
    /// <response code='200'>Successfully Deleted User</response>
    /// <response code='400'>If the user tries to delete a user that doesn't exist</response>
    /// <response code='401'>If the user isn't logged in</response>
    /// <response code='403'>If the user tries to delete a user he's not permitted to</response>
    [HttpDelete("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Produces(typeof(ApiResponse<object>))]
    [Authorize]
    public IActionResult Delete([FromRoute] string userId)
    {
        try
        {
            _userService.Delete(userId);
            return Ok(new ApiResponse<object>(ResponseStatus.Success, null, new string[] { }));
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            ObjectResult exception = e switch
            {
                CustomException
                    => BadRequest(
                        new ApiResponse<object>(
                            ResponseStatus.Error,
                            null,
                            new string[] { e.Message }
                        )
                    ),
                ForbiddenException
                    => new Forbidden(
                        new ApiResponse<object>(
                            ResponseStatus.Error,
                            null,
                            new string[] { e.Message }
                        )
                    ),
                _
                    => new InternalServerError(
                        new ApiResponse<object>(
                            ResponseStatus.Error,
                            null,
                            new string[] { "UnknownError" }
                        )
                    )
            };

            return exception;
        }
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
    public IActionResult Get([FromRoute] string userId)
    {
        try
        {
            User user = _userService.GetUserById(userId);
            UserResponseDto userResponseDto = _userMapper.UserToUserResponseDto(user);

            return Ok(
                new ApiResponse<UserResponseDto>(
                    ResponseStatus.Success,
                    userResponseDto,
                    new string[] { }
                )
            );
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            ObjectResult exception = e switch
            {
                CustomException
                    => BadRequest(
                        new ApiResponse<object>(
                            ResponseStatus.Error,
                            null,
                            new string[] { e.Message }
                        )
                    ),
                ForbiddenException
                    => new Forbidden(
                        new ApiResponse<object>(
                            ResponseStatus.Error,
                            null,
                            new string[] { e.Message }
                        )
                    ),
                _
                    => new InternalServerError(
                        new ApiResponse<object>(
                            ResponseStatus.Error,
                            null,
                            new string[] { "UnknownError" }
                        )
                    )
            };

            return exception;
        }
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
    public IActionResult WriteMessage(
        [FromBody] MessageCreateDto messageCreateDto,
        [FromRoute] string userId
    )
    {
        try
        {
            Message message = _messageService.WriteMessageToUser(messageCreateDto, userId);
            MessageResponseDto messageResponseDto = _messageMapper.MessageToMessageResponseDto(
                message
            );

            return Ok(
                new ApiResponse<MessageResponseDto>(
                    ResponseStatus.Success,
                    messageResponseDto,
                    new string[] { }
                )
            );
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            ObjectResult exception = e switch
            {
                CustomException
                    => BadRequest(
                        new ApiResponse<object>(
                            ResponseStatus.Error,
                            null,
                            new string[] { e.Message }
                        )
                    ),
                ForbiddenException
                    => new Forbidden(
                        new ApiResponse<object>(
                            ResponseStatus.Error,
                            null,
                            new string[] { e.Message }
                        )
                    ),
                _
                    => new InternalServerError(
                        new ApiResponse<object>(
                            ResponseStatus.Error,
                            null,
                            new string[] { "UnknownError" }
                        )
                    )
            };

            return exception;
        }
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
    public IActionResult UpdateMessage(
        [FromBody] MessageUpdateDto messageUpdateDto,
        [FromRoute] string userId,
        [FromRoute] string messageId
    )
    {
        try
        {
            Message message = _messageService.UpdateUserMessage(
                messageUpdateDto,
                userId,
                messageId
            );

            MessageResponseDto messageResponseDto = _messageMapper.MessageToMessageResponseDto(
                message
            );

            return Ok(
                new ApiResponse<MessageResponseDto>(
                    ResponseStatus.Success,
                    messageResponseDto,
                    new string[] { }
                )
            );
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            ObjectResult exception = e switch
            {
                BadRequestException
                    => BadRequest(
                        new ApiResponse<object>(
                            ResponseStatus.Error,
                            null,
                            new string[] { e.Message }
                        )
                    ),
                ForbiddenException
                    => new Forbidden(
                        new ApiResponse<object>(
                            ResponseStatus.Error,
                            null,
                            new string[] { e.Message }
                        )
                    ),
                _
                    => new InternalServerError(
                        new ApiResponse<object>(
                            ResponseStatus.Error,
                            null,
                            new string[] { "UnknownError" }
                        )
                    )
            };

            return exception;
        }
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
    public IActionResult DeleteMessage([FromRoute] string userId, [FromRoute] string messageId)
    {
        try
        {
            _messageService.DeleteUserMessage(userId, messageId);
            return Ok(
                new ApiResponse<MessageResponseDto>(ResponseStatus.Success, null, new string[] { })
            );
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            ObjectResult exception = e switch
            {
                BadRequestException
                    => BadRequest(
                        new ApiResponse<object>(
                            ResponseStatus.Error,
                            null,
                            new string[] { e.Message }
                        )
                    ),
                ForbiddenException
                    => new Forbidden(
                        new ApiResponse<object>(
                            ResponseStatus.Error,
                            null,
                            new string[] { e.Message }
                        )
                    ),
                _
                    => new InternalServerError(
                        new ApiResponse<object>(
                            ResponseStatus.Error,
                            null,
                            new string[] { "UnknownError" }
                        )
                    )
            };

            return exception;
        }
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
    public IActionResult GetMessages([FromRoute] string userId)
    {
        try
        {
            ICollection<Message> messages = _messageService.GetUserMessages(userId);
            var messageResponseDtos = _messageMapper.MessageCollectionToMessageResponseDtoArray(
                messages
            );
            return Ok(
                new ApiResponse<MessageResponseDto[]>(
                    ResponseStatus.Success,
                    messageResponseDtos,
                    new string[] { }
                )
            );
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            ObjectResult exception = e switch
            {
                BadRequestException
                    => BadRequest(
                        new ApiResponse<object>(
                            ResponseStatus.Error,
                            null,
                            new string[] { e.Message }
                        )
                    ),
                ForbiddenException
                    => new Forbidden(
                        new ApiResponse<object>(
                            ResponseStatus.Error,
                            null,
                            new string[] { e.Message }
                        )
                    ),
                _
                    => new InternalServerError(
                        new ApiResponse<object>(
                            ResponseStatus.Error,
                            null,
                            new string[] { "UnknownError" }
                        )
                    )
            };

            return exception;
        }
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
    public IActionResult GetAllUserServers([FromRoute] string userId)
    {
        try
        {
            var servers = _serverService.GetAllServersUserIsMemberOf(userId);
            var serverResponseDtoList = new ServerMapper().ServerCollectionToServerResponseDtoList(
                servers
            );
            return Ok(
                new ApiResponse<ServerResponseDto[]>(
                    ResponseStatus.Success,
                    serverResponseDtoList,
                    new string[] { }
                )
            );
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            ObjectResult exception = e switch
            {
                BadRequestException
                or InvalidDataException
                    => BadRequest(
                        new ApiResponse<object>(
                            ResponseStatus.Error,
                            null,
                            new string[] { e.Message }
                        )
                    ),
                _
                    => BadRequest(
                        new ApiResponse<object>(
                            ResponseStatus.Error,
                            null,
                            new string[] { "UnknownError" }
                        )
                    ),
            };

            return exception;
        }
    }
}
