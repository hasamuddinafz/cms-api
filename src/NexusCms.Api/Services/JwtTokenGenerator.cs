using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Entities;

namespace NexusCms.Api.Services;

public class JwtTokenGenerator(IConfiguration configuration) : IJwtTokenGenerator
{
    public string GenerateAccessToken(ApplicationUser user, IEnumerable<string> permissions)
    {
        var key     = configuration["Jwt:Key"]      ?? throw new InvalidOperationException("Jwt:Key is not configured.");
        var issuer  = configuration["Jwt:Issuer"]   ?? "NexusCms";
        var audience = configuration["Jwt:Audience"] ?? "NexusCms";
        var minutes = configuration.GetValue<int>("Jwt:AccessTokenExpirationMinutes", 15);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub,   user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti,   Guid.NewGuid().ToString()),
            new("fullName",                    user.FullName),
        };

        // Embed each permission as a separate claim so policy handlers can read them
        claims.AddRange(permissions.Select(p => new Claim("permission", p)));

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var creds      = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer:   issuer,
            audience: audience,
            claims:   claims,
            expires:  DateTime.UtcNow.AddMinutes(minutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes);
    }
}
