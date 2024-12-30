using System.ComponentModel.DataAnnotations;
using Netzwerk.Model;
namespace Netzwerk.DTOs
{
    public class UsersDto
    {
        public int Id { get; set; }

        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required string LastName { get; set; }

        [Required]
        public required string Email { get; set; }

        [Required]
        public List<int> KeywordIds { get; set; } = new List<int>();

        public required string Role { get; set; } = "user";
        public string? Position { get; set; }

        public string? Title { get; set; }
        public string? SecondaryTitle { get; set; }
        public string? Tel { get; set; }
        public string? Website { get; set; }
        public string? Activities { get; set; }
        public string? AdditionalLinks { get; set; }
        public string? FieldOfResearch { get; set; }
        public string? ProfileImageSource { get; set; }
        public string? PublicEmail { get; set; }
        public bool? Verified { get; set; }=false;
        public string? VerifyToken { get; set; }

        public required string GeoLocationLat { get; set; }
        public required string GeoLocationLon { get; set; }

    }
}