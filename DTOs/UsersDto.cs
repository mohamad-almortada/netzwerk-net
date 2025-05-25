using System.ComponentModel.DataAnnotations;
using Netzwerk.Model;

namespace Netzwerk.DTOs;

public class UsersDto
{
    public required string Username { get; set; }
    public string Email { get; set; } = string.Empty;
}