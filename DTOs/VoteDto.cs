namespace Netzwerk.DTOs;

public class VoteDto
{
    public int Id { get; set; }
    public int MarkerId { get; set; }
    public int UserId { get; set; }
    public short VoteValue { get; set; }
}