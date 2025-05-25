using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Netzwerk.DTOs;
using Netzwerk.Interfaces;

namespace Netzwerk.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MapController(IMapService mapService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MapDto>>> GetMap()
    {
        var markers = await mapService.GetMapsAsync();
        return Ok(markers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MapDto>> GetMap(int id)
    {
        var marker = await mapService.GetMapAsync(id);
        if (marker == null) return NotFound();

        return Ok(marker);
    }


    [HttpPost]
    public async Task<IActionResult> PostMap(MapDto markerDto)
    {
        try
        {
            var markerResponse = await mapService.CreateMapAsync(markerDto);
            return Ok(markerResponse);
        }
        catch (DbUpdateException e)
        {
            return BadRequest(e.InnerException?.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMap(int id)
    {
        var marker = await mapService.DeleteMapAsync(id);
        if (marker == false) return NotFound();

        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutMap(int id, MapDto marker)
    {
        try
        {
            var myMap = await mapService.UpdateMapAsync(id, marker);

            return Ok(myMap);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}