using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ChatLPCommon.Dtos;

namespace Chat.API.Controllers;

/// <summary>
/// Provides the REST Endpoints for the User Endpoints such as Login, Registering and user editing
/// </summary>
[ApiController]
[ApiVersion("1")]
[Route("/api/v{version:apiVersion}/[controller]")]
public class UserController : ControllerBase
{
    // TODO: Add Correct Request and Response DTOs
    
    /// <summary> Authenticates the User with a JWT Token </summary>
    /// <response code='200'>Successfully generated JWT Token</response>
    /// <response code='401'>Wrong Email, Username or Password</response>
    /// <response code='400'>Invalid Email or too Short Password</response>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Produces(typeof(UserDto))]
    [Consumes(MediaTypeNames.Application.Json)]
    public string Login()
    {
        return "Not Implemented";
    }


    /// <summary>Creates a new User</summary>
    /// <response code='200'>Successfully generated User</response>
    /// <response code='400'>Invalid Email or password too Short Password</response>
    /// <response code='409'>User with Email or Username already exists</response>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public string Register()
    {
        return "Not Implemented";
    }

    /// <summary>Updates a User</summary>
    /// <response code='200'>Successfully Updated User</response>
    /// <response code='401'></response>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public string Update()
    {
        return "Not Implemented";
    }

    [HttpDelete]
    public string Delete()
    {
        return "Not Implemented";
    }

    [HttpGet]
    [MapToApiVersion("1")]
    public string Get()
    {
        return "String";
    }
}