using System.Net.Mime;
using Chat.Application.Contracts.Repositories;
using Chat.Common.Dtos;
using Chat.Common.Types;
using Chat.Domain.Entities;
using Chat.Infrastructure.Database;
using Chat.Infrastructure.Database.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat.API.Controllers;

/// <summary>
/// Provides the REST Endpoints for Friends, such as adding friends, removing friends, and getting a friendlist
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("/api/v{version:apiVersion}/users")]
[Produces(MediaTypeNames.Application.Json)]
public class FriendsController
{
    private IGenericRepository<Friends> _genericRepository;

    /// <summary>
    /// Dependency Injection
    /// </summary>
    public FriendsController(ChatDataContext chatDataContext)
    {
        _genericRepository = new GenericRepository<Friends>(chatDataContext);
    }

    /// <summary>Adds a Friend</summary>
    /// <response code='200'>Successfully Added Friend</response>
    /// <response code='401'>If the user isn't logged in</response>
    /// <response code='403'>If the user tries to add a friend he's not permitted to</response>
    [HttpPost("{userId}/[controller]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Produces(typeof(ApiResponse<FriendsResponseDto>))]
    [Consumes(typeof(FriendsRequestDto), MediaTypeNames.Application.Json)]
    [Authorize]
    public string Add([FromBody] FriendsRequestDto friendsRequestDto)
    {
        return "Not Implemented";
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
    public string Remove()
    {
        return "Not Implemented";
    }

    /// <summary>Gets the friend list</summary>
    /// <response code='200'>Successfully Get Server</response>
    /// <response code='401'>If the user isn't logged in</response>
    /// <response code='403'>If the user tries to get a friend list he's not permitted to</response>
    [HttpGet("{userId}/[controller]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Produces(typeof(ApiResponse<FriendsResponseDto[]>))]
    [Authorize]
    public string Get()
    {
        return "String";
    }
}
