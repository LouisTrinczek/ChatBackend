using System.Net.Mime;
using Chat.Application.Contracts.Repositories;
using Chat.Application.Contracts.Services;
using Chat.Application.Exceptions;
using Chat.Application.Mappers;
using Chat.Common.Dtos;
using Chat.Common.Types;
using Chat.Domain.Entities;
using Chat.Infrastructure.Database;
using Chat.Infrastructure.Database.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Chat.API.Controllers;

/// <summary>
/// Provides the REST Endpoints for Friends, such as adding friends, removing friends, and getting a friendlist
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("/api/v{version:apiVersion}/users")]
[Produces(MediaTypeNames.Application.Json)]
public class FriendsController : ControllerBase
{
    private readonly ILogger<FriendsController> _logger;
    private readonly IFriendsService _friendsService;
    private readonly UserMapper _userMapper = new UserMapper();

    /// <summary>
    /// Dependency Injection
    /// </summary>
    public FriendsController(IFriendsService friendsService, ILogger<FriendsController> logger)
    {
        _friendsService = friendsService;
        _logger = logger;
    }

    /// <summary>Adds a Friend</summary>
    /// <response code='200'>Successfully Added Friend</response>
    /// <response code='401'>If the user isn't logged in</response>
    /// <response code='403'>If the user tries to add a friend he's not permitted to</response>
    [HttpPost("{userId}/[controller]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Produces(typeof(ApiResponse<UserResponseDto>))]
    [Consumes(typeof(FriendsRequestDto), MediaTypeNames.Application.Json)]
    [Authorize]
    public IActionResult Add(
        [FromBody] FriendsRequestDto friendsRequestDto,
        [FromRoute] string userId
    )
    {
        try
        {
            User friend = _friendsService.AddFriend(userId, friendsRequestDto.FriendId);

            var responseDto = _userMapper.UserToUserResponseDto(friend);

            return Ok(
                new ApiResponse<UserResponseDto>(
                    ResponseStatus.Success,
                    responseDto,
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

    /// <summary>Removes a Friend</summary>
    /// <response code='200'>Successfully removed a friend</response>
    /// <response code='401'>If the user isn't logged in</response>
    [HttpDelete("{userId}/[controller]/{friendId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Produces(typeof(ApiResponse<string>))]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    public IActionResult Remove([FromRoute] string userId, [FromRoute] string friendId)
    {
        try
        {
            _friendsService.RemoveFriend(userId, friendId);

            return Ok(
                new ApiResponse<UserResponseDto>(ResponseStatus.Success, null, new string[] { })
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

    /// <summary>Gets the friend list</summary>
    /// <response code='200'>Successfully Get Server</response>
    /// <response code='401'>If the user isn't logged in</response>
    /// <response code='403'>If the user tries to get a friend list he's not permitted to</response>
    [HttpGet("{userId}/[controller]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Produces(typeof(ApiResponse<UserResponseDto[]>))]
    [Authorize]
    public string Get()
    {
        return "String";
    }
}
