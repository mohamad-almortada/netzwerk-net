using System.ComponentModel.DataAnnotations;
using Netzwerk.Model;

namespace Netzwerk.DTOs;

public class UsersDto
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public UsersDto CreateCopy(UsersDto userDto)
    {
        return new UsersDto
        {
            Username = userDto.Username,
            Email = userDto.Email,
            Password = userDto.Password
        };
    }
}