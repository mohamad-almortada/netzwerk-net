namespace Netzwerk.DTOs;

public class MarkerDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Lat { get; set; } = string.Empty;
    public string Lon { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int? VerifiedBy { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public int MapId { get; set; }
    public int UserId { get; set; }
}