using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Netzwerk.DTOs;
using Netzwerk.Interfaces;

namespace Netzwerk.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]

public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UsersDto>>> GetUsers()
    {
        var users = await userService.GetUsersAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UsersDto>> GetUser(int id)
    {
        var user = await userService.GetUserAsync(id);
        if (user == null) return NotFound();

        return Ok(user);
    }

    [HttpGet("{latitude}/{longitude}")]
    public async Task<ActionResult<UsersDto>> GetUserByCoodrinate(string latitude, string longitude)
    {
        var user = await userService.GetUserAsync(latitude, longitude);
        if (user == null) return NotFound();

        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> PostUser(UsersDto userDto)
    {
        try
        {
            var userResponse = await userService.CreateUserAsync(userDto);
            return Ok(userResponse);
        }
        catch (DbUpdateException e)
        {
            return BadRequest(e.InnerException?.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await userService.DeleteUserAsync(id);
        if (user == false) return NotFound();

        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(int id, UsersDto user)
    {
        try
        {
            var myUser = await userService.UpdateUserAsync(id, user);

            return Ok(myUser);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}