namespace Netzwerk.Services;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public class JwtTokenService
{
    private readonly string _key;
    public JwtTokenService(IConfiguration cfg, string key) => _key = cfg["JWT:Key"] ?? throw new ArgumentNullException($"JWT:Key");

    public string CreateToken(IEnumerable<Claim> claims, TimeSpan validFor)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.Add(validFor),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
