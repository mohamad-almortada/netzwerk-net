using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Netzwerk.DTOs;
using Netzwerk.Service;

namespace Netzwerk.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<UsersDto>>> GetUsers()
    {
        var users = await _userService.GetUsersAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UsersDto>> GetUser(int id)
    {
        var user = await _userService.GetUserAsync(id);
        if (user == null) return NotFound();

        return Ok(user);
    }

    [HttpGet("{latitude}/{longitude}")]
    public async Task<ActionResult<UsersDto>> GetUserByCoodrinate(string latitude, string longitude)
    {
        var user = await _userService.GetUserAsync(latitude, longitude);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> PostUser(UsersDto userDto)
    {
        try
        {
            var userResponse = await _userService.CreateUserAsync(userDto);
            return Ok(userResponse);
        }
        catch (DbUpdateException e)
        {
            return BadRequest(e.InnerException.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _userService.DeleteUserAsync(id);
        if (user == false)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(int id, UsersDto user)
    {
        try
        {
            var myUser = await _userService.UpdateUserAsync(id, user);

            return Ok(myUser);
        }
        catch (UserServiceException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}