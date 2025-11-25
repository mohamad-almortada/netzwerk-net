using System.ComponentModel.DataAnnotations;

namespace Netzwerk.Model;

public class Marker
{
    public int Id { get; set; }
    public int MapId { get; set; }
    public int? UserId { get; set; }
    [MaxLength(64)] public string Title { get; set; } = string.Empty;
    [MaxLength(256)] public string Description { get; set; } = string.Empty;
    public string Lat { get; set; } = string.Empty;
    public string Lon { get; set; } = string.Empty;
    [MaxLength(32)] public string Status { get; set; } = string.Empty;
    public int? VerifiedBy { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Map Map { get; set; } = null!;
    public User User { get; set; } = null!;
    public User Verifier { get; set; } = null!;
    public IList<Category> Categories { get; set; } = [];
    public ICollection<Vote> Votes { get; set; } = [];
}