using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Netzwerk.DTOs;
using Netzwerk.Interfaces;

namespace Netzwerk.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class VoteController(IVoteService voteService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<VoteDto>>> GetVote()
    {
        var markers = await voteService.GetVotesAsync();
        return Ok(markers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<VoteDto>> GetVote(int id)
    {
        var marker = await voteService.GetVoteAsync(id);
        if (marker == null) return NotFound();

        return Ok(marker);
    }
    

    [HttpPost]
    public async Task<IActionResult> PostVote(VoteDto markerDto)
    {
        try
        {
            var markerResponse = await voteService.CreateVoteAsync(markerDto);
            return Ok(markerResponse);
        }
        catch (DbUpdateException e)
        {
            return BadRequest(e.InnerException?.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVote(int id)
    {
        var marker = await voteService.DeleteVoteAsync(id);
        if (marker == false) return NotFound();

        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutVote(int id, VoteDto marker)
    {
        try
        {
            var myVote = await voteService.UpdateVoteAsync(id, marker);

            return Ok(myVote);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}