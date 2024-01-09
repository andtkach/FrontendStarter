using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.Data;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService
{
    public string GenerateToken()
    {
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, DemoData.UserEmail),
                new Claim(ClaimTypes.Name, DemoData.UserName)
            };

        claims.Add(new Claim(ClaimTypes.Role, DemoData.UserRole));
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(DemoData.TestSecret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var tokenOptions = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }
}