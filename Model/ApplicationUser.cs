namespace Netzwerk.Model;

using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    public bool IsGuest { get; set; } = false;
}
