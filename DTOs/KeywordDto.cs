using System.ComponentModel.DataAnnotations;

namespace Netzwerk.DTOs
{
    public class KeywordDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}