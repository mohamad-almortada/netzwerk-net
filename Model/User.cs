using System.ComponentModel.DataAnnotations;

namespace Netzwerk.Model;

public class User
{
    public int Id { get; set; }
    [MaxLength(256)] public string Username { get; set; } = string.Empty;
    [MaxLength(256)] public string Email { get; set; } = string.Empty;
    [MaxLength(256)]public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<Map> Maps { get; set; } = [];
    public ICollection<Marker> Markers { get; set; } = [];
    public ICollection<Vote> Votes { get; set; } = [];
}