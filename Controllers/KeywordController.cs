using Microsoft.AspNetCore.Mvc;
using Netzwerk.DTOs;
using Netzwerk.Model;
using Netzwerk.Service;

namespace Netzwerk.Controllers;

[Route("api/[controller]")]
[ApiController]
public class KeywordController : ControllerBase
{
    private readonly IKeywordService _keywordService;

    public KeywordController(IKeywordService keywordService)
    {
        _keywordService = keywordService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<KeywordDto>>> GetKeywords()
    {
        var keywords = await _keywordService.GetKeywordsAsync();
        return Ok(keywords);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetKeyword(int id)
    {
        var keyword = await _keywordService.GetKeywordAsync(id);

        if (keyword == null)
        {
            return NotFound();
        }

        return Ok(keyword);
    }

    [HttpPost]
    public async Task<IActionResult> PostKeyword(KeywordDto keywordDto)
    {
        var keyword = await _keywordService.CreateKeywordAsync(keywordDto);
        return Ok(keyword);
    }

    [HttpPut("{id}")]
    public IActionResult PutKeyword(int id, Keyword keyword)
    {
        try
        {
            _keywordService.UpdateKeywordAsync(keyword, id);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteKeyword(int id)
    {
        var keyword = await _keywordService.DeleteKeywordAsync(id);
        if (keyword == false)
        {
            return NotFound();
        }

        return NoContent();
    }
}