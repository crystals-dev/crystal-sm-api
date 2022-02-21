using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CrystalApi.Models;
using Microsoft.IdentityModel.Tokens;

namespace CrystalApi.Services;

public class TokenService
{
    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("a3827dae-861d-4a0d-876e-671970daafc8");
        var claims = new List<Claim> {new Claim(ClaimTypes.Name, user.Email)};
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(12),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}