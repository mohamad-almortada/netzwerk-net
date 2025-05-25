namespace Netzwerk.DTOs;

public class MarkerDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Lat { get; set; }
    public decimal Lon { get; set; }
    public string Status { get; set; } = string.Empty;
    public int? VerifiedBy { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public int MapId { get; set; }
    public int UserId { get; set; }
}