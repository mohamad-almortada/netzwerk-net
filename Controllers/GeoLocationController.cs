using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netzwerk.DTOs;
using Netzwerk.Service;

namespace Netzwerk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeoLocationController : Controller
    {
        private readonly IGeoLocationService _geoLocationService;
        private readonly ILogger<GeoLocationController> _logger;

        public GeoLocationController(IGeoLocationService geoLocationService, ILogger<GeoLocationController> logger)
        {
            _geoLocationService = geoLocationService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GeoLocationDto>>> GetGeoLocations()
        {
            var geoLocations = await _geoLocationService.GetGeoLocationsAsync();
            return Ok(geoLocations);
        }

        [HttpPost]
        public async Task<IActionResult> PostGeoLocation(GeoLocationDto geoLocationDto)
        {
            var geoLocation = await _geoLocationService.CreateGeoLocationAsync(geoLocationDto);
            return Ok(geoLocation);
        }

        [HttpPut("{latitude}/{longitude}")]
        public async Task<IActionResult> PutGeoLocation(string longitude, string latitude,
            GeoLocationDto geoLocationDto)
        {
            try
            {
                await _geoLocationService.UpdateGeoLocationAsync(geoLocationDto, longitude, latitude);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }

            return NoContent();
        }

        [HttpDelete("{latitude}/{longitude}")]
        public async Task<IActionResult> DeleteGeoLocation(string longitude, string latitude)
        {
            var result = await _geoLocationService.DeleteGeoLocationAsync(longitude, latitude);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("{latitude}/{longitude}")]
        public async Task<IActionResult> GetGeoLocation([FromRoute] string longitude, [FromRoute] string latitude)
        {
            var geoLocation = await _geoLocationService.GetGeoLocationAsync(longitude, latitude);
            if (geoLocation == null)
            {
                return NotFound();
            }

            return Ok(geoLocation);
        }
    }
}