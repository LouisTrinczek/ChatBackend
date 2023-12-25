using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat.API.Controllers;

/// <summary>
/// Provides the REST Endpoints for Friends, such as adding friends, removing friends, and getting a friendlist
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("/api/v{version:apiVersion}/users/[controller]")]
public class FriendsController
{
    /// <summary>Adds a Friend</summary>
    /// <response code='200'>Successfully Added Friend</response>
    /// <response code='401'>If the user isn't logged in</response>
    /// <response code='403'>If the user tries to add a friend he's not permitted to</response>
    [HttpPost("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    public string Add()
    {
        return "Not Implemented";
    }
    
    /// <summary>Removes a Friend</summary>
    /// <response code='200'>Successfully removed a friend</response>
    /// <response code='401'>If the user isn't logged in</response>
    [HttpDelete("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Produces(MediaTypeNames.Application.Json)]
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
    [HttpGet("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Produces(MediaTypeNames.Application.Json)]
    [Authorize]
    public string Get()
    {
        return "String";
    }
}