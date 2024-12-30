using Netzwerk.DTOs;

namespace Netzwerk.Service;

public interface IGeoLocationService
{
    Task<GeoLocationDto> CreateGeoLocationAsync(GeoLocationDto geoLocation);
    Task<GeoLocationDto?> GetGeoLocationAsync(string longitude, string latitude);
    Task<IEnumerable<GeoLocationDto>> GetGeoLocationsAsync();
    Task UpdateGeoLocationAsync(GeoLocationDto geoLocationDto, string longitude, string latitude);
    Task<bool> DeleteGeoLocationAsync(string longitude, string latitude);
}