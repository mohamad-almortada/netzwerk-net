using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Netzwerk.DTOs;
using Netzwerk.Interfaces;

namespace Netzwerk.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MarkerController(IMarkerService markerService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MarkerDto>>> GetMarker()
    {
        var markers = await markerService.GetMarkersAsync();
        return Ok(markers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MarkerDto>> GetMarker(int id)
    {
        var marker = await markerService.GetMarkerAsync(id);
        if (marker == null) return NotFound();

        return Ok(marker);
    }

    [HttpGet("{latitude}/{longitude}")]
    public async Task<ActionResult<MarkerDto>> GetMarkerByCoodrinate(string latitude, string longitude)
    {
        var marker = await markerService.GetMarkerAsync(latitude, longitude);
        if (marker == null) return NotFound();

        return Ok(marker);
    }

    [HttpPost]
    public async Task<IActionResult> PostMarker(MarkerDto markerDto)
    {
        try
        {
            var markerResponse = await markerService.CreateMarkerAsync(markerDto);
            return Ok(markerResponse);
        }
        catch (DbUpdateException e)
        {
            return BadRequest(e.InnerException?.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMarker(int id)
    {
        var marker = await markerService.DeleteMarkerAsync(id);
        if (marker == false) return NotFound();

        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutMarker(int id, MarkerDto marker)
    {
        try
        {
            var myMarker = await markerService.UpdateMarkerAsync(id, marker);

            return Ok(myMarker);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}