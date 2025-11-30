using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Netzwerk.Data;
using Netzwerk.Model;
using Netzwerk.Services;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Netzwerk.Controllers;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class AuthController(JwtTokenService jwt, UserManager<ApplicationUser> um, ApiContext db, IOptions<JwtOptions> _jwtOptions)
    : ControllerBase
{
    
     [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var user = db.Users.SingleOrDefault(u => u.Username == request.Username);
        if (user == null)
            return Unauthorized("Invalid username or password");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash.ToString()))
            return Unauthorized("Invalid username or password");

        var token = GenerateJwtToken(user);
        return Ok(new { token });
    }

    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Value.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim("uid", user.Id.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Value.Issuer,
            audience: _jwtOptions.Value.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(_jwtOptions.Value.ExpiresHours),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    [HttpPost("guest")]
    public IActionResult Guest()
    {
        var jti = Guid.NewGuid().ToString("N");
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, $"guest_{jti}"),
            new Claim("role", "Guest"),
            new Claim("guest", "true"),
            new Claim(JwtRegisteredClaimNames.Jti, jti)
        };

        var token = jwt.CreateToken(claims, TimeSpan.FromHours(1));
        return Ok(new { token, expires = DateTime.UtcNow.AddHours(1) });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var permanentCount = await db.Users.CountAsync();
        if (permanentCount >= 50) return BadRequest("Permanent user registration is closed (limit reached).");

        var user = new ApplicationUser { UserName = dto.Email, Email = dto.Email, IsGuest = false };
        var res = await um.CreateAsync(user, dto.Password);
        if (!res.Succeeded) return BadRequest(res.Errors);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim("role", "User")
        };
        var token = jwt.CreateToken(claims, TimeSpan.FromDays(30)); 
        return Ok(new { token, userId = user.Id });
    }
}

public record RegisterDto(string Email, string Password);
public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; }  = string.Empty;
}