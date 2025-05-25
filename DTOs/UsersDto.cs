using System.ComponentModel.DataAnnotations;
using Netzwerk.Model;

namespace Netzwerk.DTOs;

public class UsersDto
{
    public required string Username { get; set; }
    public string Email { get; set; } = string.Empty;

    public UsersDto CreateCopy(UsersDto userDto)
    {
        return new UsersDto
        {
            Username = userDto.Username,
            Email = userDto.Email
        };
    }
}