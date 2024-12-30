namespace Netzwerk.Model
{
    public class Keyword
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public List<UserKeyword> UserKeywords { get; } = [];
    }
}