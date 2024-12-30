namespace Netzwerk.DTOs
{
    public class GeoLocationDto {
        public required string Latitude { get; set; }
        public required string Longitude { get; set; }
        public required string AddressName { get; set; }
        public required string Street { get; set; }
        public string? BuildingNr { get; set; }
        public string? Plz { get; set; }
        public required string City { get; set; }
        public string? Typ { get; set; }
    }
}