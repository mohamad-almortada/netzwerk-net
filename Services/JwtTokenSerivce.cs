namespace Netzwerk.Services;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public class JwtTokenService(IConfiguration cfg)
{
    public string CreateToken(IEnumerable<Claim> claims, TimeSpan validFor, string issuer, string audience, string key)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            audience: audience,
            issuer: issuer,
            expires: DateTime.UtcNow.Add(validFor),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
