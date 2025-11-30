using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Netzwerk.DTOs;
using Netzwerk.Interfaces;

namespace Netzwerk.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoryController(ICategoryService categoryService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategory()
    {
        var markers = await categoryService.GetCategoriesAsync();
        return Ok(markers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetCategory(int id)
    {
        var marker = await categoryService.GetCategoryAsync(id);
        if (marker == null) return NotFound();

        return Ok(marker);
    }
    

    [HttpPost]
    public async Task<IActionResult> PostCategory(CategoryDto markerDto)
    {
        try
        {
            var markerResponse = await categoryService.CreateCategoryAsync(markerDto);
            return Ok(markerResponse);
        }
        catch (DbUpdateException e)
        {
            return BadRequest(e.InnerException?.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var marker = await categoryService.DeleteCategoryAsync(id);
        if (marker == false) return NotFound();

        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutCategory(int id, CategoryDto marker)
    {
        try
        {
            var myCategory = await categoryService.UpdateCategoryAsync(id, marker);

            return Ok(myCategory);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}