using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Iron.Aplication.Services;
using Iron.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Iron.Infra.Services;

public class BearerJwtTokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public BearerJwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public string GenerateToken(User user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");

        var secret = jwtSettings["SecretKey"]
            ?? throw new InvalidOperationException("JwtSettings:SecretKey não configurada.");

        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email.Value),
            new(JwtRegisteredClaimNames.Name, user.FirstName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("platform_admin", user.IsPlatformAdmin.ToString())
        };

        var expirationTimeInMinutes = jwtSettings.GetValue<int>("ExpirationTimeInMinutes");

        var token = new JwtSecurityToken(
            issuer: jwtSettings.GetValue<string>("Issuer"),
            audience: jwtSettings.GetValue<string>("Audience"),
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationTimeInMinutes),
            signingCredentials: new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


    public (string RefreshToken, DateTime RefreshExpiresAt) GenerateRefreshToken()
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");

        var refreshExpirationTimeInMinutes = jwtSettings.GetValue<int>("RefreshExpirationTimeInMinutes");
        var expiresAt = DateTime.UtcNow.AddMinutes(refreshExpirationTimeInMinutes);

        var tokenString = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        return (tokenString, expiresAt);
    }
}
