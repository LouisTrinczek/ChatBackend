using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat.API.Controllers;

/// <summary>
/// Provides the REST Endpoints for the Server Channels like Creation, Deletion and Updating
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("/api/v{version:apiVersion}/servers/{serverId}/[controller]")]
public class ChannelsController
{
    /// <summary>Creates a Channel</summary>
    /// <response code='200'>Successfully Created Channel</response>
    /// <response code='401'>If the user isn't logged in</response>
    /// <response code='403'>If the user tries to create a Channel he's not permitted to</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    public string Create()
    {
        return "Not Implemented";
    }
    
    /// <summary>Updates a Channel</summary>
    /// <response code='200'>Successfully Updated Channel</response>
    /// <response code='401'>If the user isn't logged in</response>
    /// <response code='403'>If the user tries to update a Channel he's not permitted to</response>
    [HttpPut("{channelId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    public string Update()
    {
        return "Not Implemented";
    }

    /// <summary>Deletes a Channel</summary>
    /// <response code='200'>Successfully Deleted Channel</response>
    /// <response code='401'>If the user isn't logged in</response>
    /// <response code='403'>If the user tries to delete a Channel he's not permitted to</response>
    [HttpDelete("{channelId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    public string Delete()
    {
        return "Not Implemented";
    }

    /// <summary>Gets a Channel</summary>
    /// <response code='200'>Successfully Get Channel</response>
    /// <response code='401'>If the user isn't logged in</response>
    /// <response code='403'>If the user tries to get a Channel he's not permitted to</response>
    [HttpGet("{channelId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Produces(MediaTypeNames.Application.Json)]
    [Authorize]
    public string Get()
    {
        return "String";
    }
    
    /// <summary>Writes a Message to a Channel</summary>
    /// <response code='200'>Successfully sent message</response>
    /// <response code='401'>If the user isn't logged in</response>
    /// <response code='403'>If the user tries to write in a channel he's not permitted to</response>
    [HttpPost("{channelId}/messages")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ApiExplorerSettings(GroupName = "Channel Messages")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Produces(MediaTypeNames.Application.Json)]
    [Authorize]
    public string WriteMessage()
    {
        return "String";
    }
    
    /// <summary>Updates a Message</summary>
    /// <response code='200'>Successfully updated message</response>
    /// <response code='401'>If the user isn't logged in</response>
    /// <response code='403'>If the user tries to update a message he's not permitted to</response>
    [HttpPatch("{channelId}/messages/{messageId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ApiExplorerSettings(GroupName = "Channel Messages")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Produces(MediaTypeNames.Application.Json)]
    [Authorize]
    public string UpdateMessage()
    {
        return "String";
    }
    
    /// <summary>Deletes A Message</summary>
    /// <response code='200'>Successfully deleted message</response>
    /// <response code='401'>If the user isn't logged in</response>
    /// <response code='403'>If the user tries to delete a message he's not permitted to</response>
    [HttpDelete("{channelId}/messages/{messageId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ApiExplorerSettings(GroupName = "Channel Messages")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Produces(MediaTypeNames.Application.Json)]
    [Authorize]
    public string DeleteMessage()
    {
        return "String";
    }
}