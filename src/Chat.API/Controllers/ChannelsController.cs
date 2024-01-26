using System.Data;
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
    private readonly IMessageService _messageService;
    private readonly ChannelMapper _channelMapper = new ChannelMapper();
    private readonly MessageMapper _messageMapper = new MessageMapper();
    private readonly ILogger<ChannelsController> _logger;

    /// <summary>
    /// Dependency Injection
    /// </summary>
    public ChannelsController(
        ChatDataContext chatDataContext,
        IChannelService channelService,
        IMessageService messageService,
        ILogger<ChannelsController> logger
    )
    {
        _genericRepository = new GenericRepository<Channel>(chatDataContext);
        _channelService = channelService;
        _messageService = messageService;
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

    /// <summary>Updates a Channel</summary>
    /// <response code='200'>Successfully Updated Channel</response>
    /// <response code='400'>If the user tries to access a channel that is not part of the server</response>
    /// <response code='401'>If the user isn't logged in</response>
    /// <response code='403'>If the user tries to update a Channel he's not permitted to</response>
    [HttpPut("{channelId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Consumes(typeof(ServerChannelUpdateDto), MediaTypeNames.Application.Json)]
    [Produces(typeof(ApiResponse<ServerChannelResponseDto>))]
    [Authorize]
    public IActionResult Update(
        [FromBody] ServerChannelUpdateDto serverChannelUpdateDto,
        [FromRoute] string serverId,
        [FromRoute] string channelId
    )
    {
        try
        {
            Channel channel = _channelService.Update(serverChannelUpdateDto, serverId, channelId);
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

    /// <summary>Deletes a Channel</summary>
    /// <response code='200'>Successfully Deleted Channel</response>
    /// <response code='400'>If the user tries to access a channel that is not part of the server</response>
    /// <response code='401'>If the user isn't logged in</response>
    /// <response code='403'>If the user tries to delete a Channel he's not permitted to</response>
    [HttpDelete("{channelId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Produces(typeof(ApiResponse<string>))]
    [Authorize]
    public IActionResult Delete([FromRoute] string serverId, [FromRoute] string channelId)
    {
        try
        {
            _channelService.Delete(serverId, channelId);
            return Ok(new ApiResponse<object>(ResponseStatus.Success, null, new string[] { }));
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
    public IActionResult Get([FromRoute] string serverId, [FromRoute] string channelId)
    {
        try
        {
            var channel = _channelService.GetChannelById(serverId, channelId);
            var channelResponseDto = _channelMapper.ServerChannelToChannelResponseDto(channel);
            return Ok(
                new ApiResponse<ServerChannelResponseDto>(
                    ResponseStatus.Success,
                    channelResponseDto,
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
    public IActionResult GetAllServerChannels([FromRoute] string serverId)
    {
        try
        {
            var channel = _channelService.GetAllChannelsOfServer(serverId);
            var channelResponseDto = _channelMapper.ServerChannelCollectionToChannelResponseDtoList(
                channel
            );
            return Ok(
                new ApiResponse<ServerChannelResponseDto[]>(
                    ResponseStatus.Success,
                    channelResponseDto,
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
    public IActionResult WriteMessage(
        [FromBody] MessageCreateDto messageCreateDto,
        [FromRoute] string serverId,
        [FromRoute] string channelId
    )
    {
        try
        {
            Message message = _messageService.WriteMessageToChannel(
                messageCreateDto,
                serverId,
                channelId
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
                    )
                    {
                        StatusCode = StatusCodes.Status403Forbidden
                    },
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
    [HttpPatch("{channelId}/messages/{messageId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ApiExplorerSettings(GroupName = "Channel Messages")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Produces(typeof(ApiResponse<MessageResponseDto>))]
    [Consumes(typeof(MessageUpdateDto), MediaTypeNames.Application.Json)]
    [Authorize]
    public IActionResult UpdateMessage(
        [FromBody] MessageUpdateDto messageUpdateDto,
        [FromRoute] string serverId,
        [FromRoute] string channelId,
        [FromRoute] string messageId
    )
    {
        try
        {
            Message message = _messageService.UpdateChannelMessage(
                messageUpdateDto,
                serverId,
                channelId,
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
                    )
                    {
                        StatusCode = StatusCodes.Status403Forbidden
                    },
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
    [HttpDelete("{channelId}/messages/{messageId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ApiExplorerSettings(GroupName = "Channel Messages")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Produces(typeof(ApiResponse<string>))]
    [Authorize]
    public IActionResult DeleteMessage(
        [FromRoute] string serverId,
        [FromRoute] string channelId,
        [FromRoute] string messageId
    )
    {
        try
        {
            _messageService.DeleteChannelMessage(serverId, channelId, messageId);

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
                    )
                    {
                        StatusCode = StatusCodes.Status403Forbidden
                    },
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
    public IActionResult GetMessages([FromRoute] string serverId, [FromRoute] string channelId)
    {
        try
        {
            ICollection<Message> messages = _messageService.GetChannelMessages(serverId, channelId);
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
}
