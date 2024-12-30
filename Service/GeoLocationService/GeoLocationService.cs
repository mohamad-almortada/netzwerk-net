using Microsoft.EntityFrameworkCore;
using Netzwerk.Data;
using Netzwerk.DTOs;
using Netzwerk.Model;

namespace Netzwerk.Service
{
    public class GeoLocationservice : IGeoLocationService
    {
        private readonly ApiContext _context;
        private readonly ILogger<GeoLocationservice> _logger;

        public GeoLocationservice(ApiContext context, ILogger<GeoLocationservice> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<GeoLocationDto> CreateGeoLocationAsync(GeoLocationDto geoLocationDto)
        {
            var geoLocationEntity = new GeoLocation
            {
                Latitude = geoLocationDto.Latitude,
                Longitude = geoLocationDto.Longitude,
                AddressName = geoLocationDto.AddressName,
                Street = geoLocationDto.Street,
                BuildingNr = geoLocationDto.BuildingNr,
                Plz = geoLocationDto.Plz,
                City = geoLocationDto.City,
                Typ = geoLocationDto.Typ
            };
            await _context.GeoLocations.AddAsync(geoLocationEntity);
            await _context.SaveChangesAsync();
            return geoLocationDto;
        }

        public async Task<bool> DeleteGeoLocationAsync(string Longitude, string Latitude)
        {
            var geoLocation = await _context.GeoLocations.FindAsync(Latitude, Longitude);
            if (geoLocation == null)
            {
                return false;
            }

            _context.GeoLocations.Remove(geoLocation);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<GeoLocationDto?> GetGeoLocationAsync(string longitude, string latitude)
        {
            var geoLocation = await _context.GeoLocations.FindAsync(latitude, longitude);
            if (geoLocation == null)
            {
                return null;
            }

            var geoLocationDto = new GeoLocationDto
            {
                Latitude = geoLocation.Latitude,
                Longitude = geoLocation.Longitude,
                AddressName = geoLocation.AddressName,
                Street = geoLocation.Street,
                BuildingNr = geoLocation.BuildingNr,
                Plz = geoLocation.Plz,
                City = geoLocation.City,
                Typ = geoLocation.Typ
            };
            return geoLocationDto;
        }

        public async Task<IEnumerable<GeoLocationDto>> GetGeoLocationsAsync()
        {
            var geoLocations = await _context.GeoLocations.ToListAsync();
            var geoLocationDto = geoLocations.Select(u => new GeoLocationDto
            {
                Latitude = u.Latitude,
                Longitude = u.Longitude,
                AddressName = u.AddressName,
                Street = u.Street,
                BuildingNr = u.BuildingNr,
                Plz = u.Plz,
                City = u.City,
                Typ = u.Typ
            });
            return geoLocationDto;
        }

        public async Task UpdateGeoLocationAsync(GeoLocationDto geoLocationDto, string Longitude, string Latitude)
        {
            var geoLocation = await _context.GeoLocations.FindAsync(Latitude, Longitude);
            if (geoLocation == null)
            {
                throw new ArgumentException("GeoLocation not found");
            }

            _context.Entry(geoLocation).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}