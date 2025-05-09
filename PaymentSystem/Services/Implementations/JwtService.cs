using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PaymentSystem.Data_Acess_Layer.Models;
using PaymentSystem.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

public class JwtService : IJwtService
{
    private readonly AppSettings _appSettings;

    public JwtService(IOptions<AppSettings> options)
    {
        _appSettings = options.Value;
    }

   

    public string GenerateJwtToken(Users user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.UserName)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Jwt.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _appSettings.Jwt.Issuer,
            audience: _appSettings.Jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15), // Access token expiry
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

   
}
