namespace Netzwerk.Model;

public class Vote
{
    public int Id { get; set; }
    public int MarkerId { get; set; }
    public int UserId { get; set; }
    public short VoteValue { get; set; }
    public DateTime CreatedAt { get; set; }

    public Marker Marker { get; set; } = null!;
    public User User { get; set; } = null!;
}