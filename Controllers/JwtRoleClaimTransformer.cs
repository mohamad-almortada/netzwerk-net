using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;

namespace Netzwerk.Controllers;

public class JwtRoleClaimTransformer : IClaimsTransformation
{
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var resourceAccessClaim = principal.FindFirst(c => c.Type == "resource_access");
        if (resourceAccessClaim == null) return Task.FromResult(principal);
        var resourceAccessJson = JsonSerializer.Deserialize<Dictionary<string, object>>(resourceAccessClaim.Value);
        if (resourceAccessJson == null || !resourceAccessJson.TryGetValue("dotnet-client", out var value))
            return Task.FromResult(principal);
        var roles = ((JsonElement)value).GetProperty("roles")
            .EnumerateArray()
            .Select(role => role.GetString())
            .ToList();

        var identity = (ClaimsIdentity)principal.Identity!;
        identity.AddClaims(roles.Select(role => new Claim(ClaimTypes.Role, role ?? string.Empty)));

        return Task.FromResult(principal);
    }
}