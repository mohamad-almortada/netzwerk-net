using System.ComponentModel.DataAnnotations;

namespace Netzwerk.Model;

public class Map
{
    public int Id { get; set; }
    public int UserId { get; set; }
    [MaxLength(64)] public string Title { get; set; } = string.Empty;
    [MaxLength(128)] public string Description { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public User User { get; set; } = null!;
    public ICollection<Marker> Markers { get; set; } = [];
}