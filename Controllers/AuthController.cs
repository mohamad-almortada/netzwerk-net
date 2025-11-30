using System.IdentityModel.Tokens.Jwt;
using System.Text;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Netzwerk.Data;
using Netzwerk.DTOs;
using Netzwerk.Interfaces;
using Netzwerk.Model;
using Netzwerk.Services;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Netzwerk.Controllers;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AuthController(
    JwtTokenService jwt,
    ApiContext db,
    IOptions<JwtOptions> jwtOptions,
    IUserService userService)
    : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var user = db.Users.SingleOrDefault(u => u.Username == request.Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash, false, HashType.SHA256))
            return Unauthorized("Invalid username or password");

        var token = jwt.CreateToken([
            new Claim("uid", user.Id.ToString()),
            new Claim("role", "User")
        ], TimeSpan.FromDays(30), jwtOptions.Value.Issuer, jwtOptions.Value.Audience, jwtOptions.Value.Key);
        return Ok(new { token });
    }

    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim("uid", user.Id.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtOptions.Value.Issuer,
            audience: jwtOptions.Value.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(jwtOptions.Value.ExpiresHours),
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

        var token = jwt.CreateToken(claims, TimeSpan.FromHours(1), jwtOptions.Value.Issuer, jwtOptions.Value.Audience, jwtOptions.Value.Key);
        return Ok(new { token, expires = DateTime.UtcNow.AddHours(1) });
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var permanentCount = await db.Users.CountAsync();
        if (permanentCount >= 50) return BadRequest("User limit reached");

        var user = await userService.CreateUserAsync(new UsersDto()
        {
            Username = dto.Email,
            Email = dto.Email,
            Password = dto.Password
        });

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("role", "User")
        };
        var token = jwt.CreateToken(claims, TimeSpan.FromDays(30), jwtOptions.Value.Issuer, jwtOptions.Value.Audience, jwtOptions.Value.Key);
        return Ok(new { token, userId = user.Id });
    }
}

public record RegisterDto(string Email, string Password);

public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}