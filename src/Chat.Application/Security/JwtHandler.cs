using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Chat.Application.Contracts.Security;
using Chat.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Chat.Application.Security;

public class JwtHandler : IJwtHandler
{
    private IConfiguration _configuration;
    private JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();
    private readonly HttpContext _httpContext;
    private readonly byte[] _key;

    public JwtHandler(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _httpContext = httpContextAccessor.HttpContext!;
        _key = Encoding.ASCII.GetBytes(_configuration["JWT:Key"]);
    }

    /// <summary>
    /// Generates a JWT Token
    /// </summary>
    /// <param name="user"></param>
    /// <returns>JWT Token</returns>
    public string GenerateToken(User user)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
                new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                }
            ),
            IssuedAt = DateTime.UtcNow,
            Issuer = _configuration["JWT:Issuer"],
            Audience = _configuration["JWT:Audience"],
            Expires = DateTime.UtcNow.AddMinutes(9999930),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(_key),
                SecurityAlgorithms.HmacSha512
            ),
        };

        var token = _tokenHandler.CreateToken(tokenDescriptor);

        var createdToken = _tokenHandler.WriteToken(token)!;

        return createdToken;
    }

    /// <summary>
    /// Validates a JWT Token
    /// </summary>
    /// <param name="token"></param>
    /// <returns>User ID if token is valid</returns>
    public string? ValidateToken(string token)
    {
        var validationParameters = new TokenValidationParameters()
        {
            IssuerSigningKey = new SymmetricSecurityKey(_key),
            ValidIssuer = _configuration["JWT:Issuer"],
            ValidAudience = _configuration["JWT:Audience"],
            ValidateLifetime = true,
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true
        };
        try
        {
            _tokenHandler.ValidateToken(
                token,
                validationParameters,
                out SecurityToken validatedToken
            );
            var jwtToken = (JwtSecurityToken)validatedToken;
            return jwtToken.Claims.FirstOrDefault(x => x.Type == "nameid")?.Value;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public string? GetAuthenticatedClaimValue(string claimType)
    {
        var claimValue = _httpContext
            .User.Claims.FirstOrDefault(claim => claim.Type == claimType)!
            .Value;
        return claimValue;
    }
}
