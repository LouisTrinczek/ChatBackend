using System.Data;
using System.Net.Mime;
using Chat.Application.Contracts.Repositories;
using Chat.Application.Contracts.Services;
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
/// Provides the REST Endpoints for the Server Channels like Creation, Deletion and Updating
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("/api/v{version:apiVersion}/servers/{serverId}/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class ChannelsController : ControllerBase
{
    private readonly IGenericRepository<Channel> _genericRepository;
    private readonly IChannelService _channelService;
    private readonly ChannelMapper _channelMapper = new ChannelMapper();
    private readonly ILogger<ChannelsController> _logger;

    /// <summary>
    /// Dependency Injection
    /// </summary>
    public ChannelsController(
        ChatDataContext chatDataContext,
        IChannelService channelService,
        ILogger<ChannelsController> logger
    )
    {
        _genericRepository = new GenericRepository<Channel>(chatDataContext);
        _channelService = channelService;
        _logger = logger;
    }

    /// <summary>Creates a Channel</summary>
    /// <response code='200'>Successfully Created Channel</response>
    /// <response code='401'>If the user isn't logged in</response>
    /// <response code='403'>If the user tries to create a Channel he's not permitted to</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Consumes(typeof(ServerChannelCreateDto), MediaTypeNames.Application.Json)]
    [Produces(typeof(ApiResponse<ServerChannelResponseDto>))]
    [Authorize]
    public IActionResult Create(
        [FromBody] ServerChannelCreateDto serverChannelCreateDto,
        [FromRoute] string serverId
    )
    {
        try
        {
            Channel channel = _channelService.Create(serverChannelCreateDto, serverId);
            ServerChannelResponseDto serverChannelResponse =
                _channelMapper.ServerChannelToChannelResponseDto(channel);

            return Ok(
                new ApiResponse<ServerChannelResponseDto>(
                    ResponseStatus.Success,
                    serverChannelResponse,
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

    /// <summary>Updates a Channel</summary>
    /// <response code='200'>Successfully Updated Channel</response>
    /// <response code='401'>If the user isn't logged in</response>
    /// <response code='403'>If the user tries to update a Channel he's not permitted to</response>
    [HttpPut("{channelId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Consumes(typeof(ServerChannelUpdateDto), MediaTypeNames.Application.Json)]
    [Produces(typeof(ApiResponse<ServerChannelResponseDto>))]
    [Authorize]
    public string Update([FromBody] ServerChannelUpdateDto serverChannelUpdateDto)
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
    [Produces(typeof(ApiResponse<string>))]
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
    [Produces(typeof(ApiResponse<ServerChannelResponseDto>))]
    [Authorize]
    public string Get()
    {
        return "String";
    }

    /// <summary>Gets all Channels within the server</summary>
    /// <response code='200'>Successfully Get all channels a user sees</response>
    /// <response code='401'>If the user isn't logged in</response>
    /// <response code='403'>If the user tries to get a Servers Channels he's not permitted to</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Produces(typeof(ApiResponse<ServerChannelResponseDto[]>))]
    [Authorize]
    public string GetAllServerChannels()
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
    [HttpPatch("{channelId}/messages/{messageId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ApiExplorerSettings(GroupName = "Channel Messages")]
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
    [HttpDelete("{channelId}/messages/{messageId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ApiExplorerSettings(GroupName = "Channel Messages")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Produces(typeof(ApiResponse<string>))]
    [Authorize]
    public string DeleteMessage()
    {
        return "String";
    }

    /// <summary>Gets a Paginated Chat from a channel</summary>
    /// <response code='200'>Successfully get chat</response>
    /// <response code='401'>If the user isn't logged in</response>
    /// <response code='403'>If the user tries to get a chat he's not permitted to</response>
    [HttpGet("{channelId}/messages")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ApiExplorerSettings(GroupName = "Channel Messages")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Produces(typeof(PaginatedApiResponse<MessageResponseDto[]>))]
    [Authorize]
    public string GetMessages()
    {
        return "String";
    }
}
