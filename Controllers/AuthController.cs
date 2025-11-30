using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Netzwerk.Data;
using Netzwerk.Model;
using Netzwerk.Services;

namespace Netzwerk.Controllers;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class AuthController(JwtTokenService jwt, UserManager<ApplicationUser> um, ApiContext db)
    : ControllerBase
{
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
