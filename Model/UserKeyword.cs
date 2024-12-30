using System.ComponentModel.DataAnnotations;

namespace Netzwerk.Model
{
    public class UserKeyword
    {
        public required int UserId { get; set; }
        public required User User { get; set; } = null!;
        public required int KeywordId { get; set; } 
        public required Keyword Keyword { get; set; } = null!;
    }
}