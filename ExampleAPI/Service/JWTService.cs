using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using ExampleAPI.Entities;
using Microsoft.IdentityModel.Tokens;

namespace ExampleAPI.Service;

public class JWTService
{
    private static SymmetricSecurityKey key = new SymmetricSecurityKey(
        RandomNumberGenerator.GetBytes(32)); //Лучше задать секретный ключ
    
    public static TokenValidationParameters validation = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = "ExampleAPI",

        ValidateAudience = true,
        ValidAudience = "ExampleAPIService",

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = key,

        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,

        NameClaimType = "name",
        RoleClaimType = "role"
    };
    
    public static string CreateToken(AppUser user)
    {
        var claims = new[]
        {
            new Claim("sub", user.ID.ToString()),
            new Claim("name", user.Login),
            new Claim("role", user.Role.Name),
        };

        var token = new JwtSecurityToken(
            issuer: "ExampleAPI",
            audience: "ExampleAPIService",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}