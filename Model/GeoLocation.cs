using System.ComponentModel.DataAnnotations;

namespace Netzwerk.Model
{
    public class GeoLocation
    {
        [Key]
        public required string Latitude { get; set; }
        [Key]
        public required string Longitude { get; set; }
        public required string AddressName { get; set; }
        public required string Street { get; set; }
        public string? BuildingNr { get; set; }
        public string? Plz { get; set; }
        public required string City { get; set; }
        public string? Typ { get; set; }
        public ICollection<User> Users { get; set; } = [];
    }
}