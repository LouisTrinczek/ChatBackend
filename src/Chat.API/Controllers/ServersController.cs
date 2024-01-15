using System.Net.Mime;
using Chat.Application.Contracts.Services;
using Chat.Application.Exceptions;
using Chat.Application.Mappers;
using Chat.Common.Dtos;
using Chat.Common.Types;
using Chat.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Chat.API.Controllers;

/// <summary>
/// Provides the REST Endpoints for the Servers such as Creation, Deletion, and Updating
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("/api/v{version:apiVersion}/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class ServersController : ControllerBase
{
    private readonly ILogger<ServersController> _logger;
    private readonly IServerService _serverService;
    private readonly ServerMapper _serverMapper = new ServerMapper();

    /// <summary>
    /// Dependency Injection
    /// </summary>
    public ServersController(ILogger<ServersController> logger, IServerService serverService)
    {
        _logger = logger;
        _serverService = serverService;
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
    public IActionResult Create([FromBody] ServerCreationDto serverCreationDto)
    {
        try
        {
            Server server = _serverService.Create(serverCreationDto);

            var serverResponseDto = _serverMapper.ServerToServerResponseDto(server);

            return Ok(
                new ApiResponse<ServerResponseDto>(
                    ResponseStatus.Success,
                    serverResponseDto,
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
    public IActionResult Update(
        [FromBody] ServerUpdateDto serverUpdateDto,
        [FromRoute] string serverId
    )
    {
        try
        {
            Server server = _serverService.Update(serverUpdateDto, serverId);
            var serverResponseDto = _serverMapper.ServerToServerResponseDto(server);

            return Ok(
                new ApiResponse<ServerResponseDto>(
                    ResponseStatus.Success,
                    serverResponseDto,
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
                            ResponseStatus.Success,
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
    public IActionResult Delete([FromRoute] string serverId)
    {
        try
        {
            _serverService.Delete(serverId);
            return Ok(
                new ApiResponse<ServerResponseDto>(ResponseStatus.Success, null, new string[] { })
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
    public IActionResult Get([FromRoute] string serverId)
    {
        try
        {
            Server server = _serverService.GetServerById(serverId);
            ServerResponseDto serverResponseDto = _serverMapper.ServerToServerResponseDto(server);

            return Ok(
                new ApiResponse<ServerResponseDto>(
                    ResponseStatus.Success,
                    serverResponseDto,
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
