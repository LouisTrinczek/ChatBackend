using System.Net.Mime;
using Chat.Common.Dtos;
using Chat.Common.Types;
using Chat.Domain;
using Chat.Domain.Entities;
using Chat.Persistence.Context;
using Chat.Persistence.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat.API.Controllers;

/// <summary>
/// Provides the REST Endpoints for the Servers such as Creation, Deletion, and Updating
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("/api/v{version:apiVersion}/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class ServersController
{
    private IGenericRepository<Server> _genericRepository;

    /// <summary>
    /// Dependency Injection
    /// </summary>
    public ServersController(ChatDataContext chatDataContext)
    {
        _genericRepository = new GenericRepository<Server>(chatDataContext);
    }
    
    /// <summary>Creates a Server</summary>
    /// <response code='200'>Successfully Created Server</response>
    /// <response code='401'>If the user isn't logged in</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Produces(typeof(ApiResponse<ServerResponseDto>))]
    [Consumes(typeof(ServerResponseDto), MediaTypeNames.Application.Json)]
    [Authorize]
    public string Create([FromBody] ServerCreationDto serverCreationDto)
    {
        return "Not Implemented";
    }

    /// <summary>Updates a Server</summary>
    /// <response code='200'>Successfully updated Server</response>
    /// <response code='401'>If the user isn't logged in</response>
    /// <response code='403'>If the user tries to update a Server he's not permitted to</response>
    [HttpPut("{serverId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Produces(typeof(ApiResponse<ServerResponseDto>))]
    [Consumes(typeof(ServerResponseDto), MediaTypeNames.Application.Json)]
    [Authorize]
    public string Update([FromBody] ServerUpdateDto serverUpdateDto)
    {
        return "Not Implemented";
    }

    /// <summary>Deletes a Server</summary>
    /// <response code='200'>Successfully Deleted Server</response>
    /// <response code='401'>If the user isn't logged in</response>
    /// <response code='403'>If the user tries to delete a Server he's not permitted to</response>
    [HttpDelete("{serverId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Produces(typeof(ApiResponse<string>))]
    [Authorize]
    public string Delete()
    {
        return "Not Implemented";
    }

    /// <summary>Gets a Server</summary>
    /// <response code='200'>Successfully Get Server</response>
    /// <response code='401'>If the user isn't logged in</response>
    /// <response code='403'>If the user tries to get a Server he's not permitted to</response>
    [HttpGet("{serverId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Produces(typeof(ApiResponse<ServerResponseDto>))]
    [Authorize]
    public string Get()
    {
        return "String";
    }
}