using ChefMate.API.Mapping;
using ChefMate.API.Models.Dto;
using ChefMate.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChefMate.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProductController(ILogger<ProductController> logger,
                        IProductService productService) : ControllerBase
{
    private readonly ILogger<ProductController> _logger = logger;
    private readonly IProductService _service = productService;
    private static readonly ProductMapper _mapper = new();

    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> GetAll()
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(userEmail))
            return Unauthorized();

        var profileId = $"profiles/{userEmail}";

        var products = await _service.GetAllAsync(profileId);
        var dtos = _mapper.ToDtoList(products);
        return Ok(dtos);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create([FromBody] ProductCreateDto dto)
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(userEmail))
            return Unauthorized();

        var profileId = $"profiles/{userEmail}";
        var product = await _service.AddAsync(dto, profileId);
        return CreatedAtAction(nameof(GetAll), new { id = product.Id }, _mapper.ToDto(product));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] ProductUpdateDto dto)
    {
        if (id != dto.Id)
            return BadRequest("Id mismatch");

        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(userEmail))
            return Unauthorized();

        var profileId = $"profiles/{userEmail}";
        var updated = await _service.UpdateAsync(dto, profileId);
        if (updated == null)
            return NotFound();

        return Ok(_mapper.ToDto(updated));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(userEmail))
            return Unauthorized();

        var profileId = $"profiles/{userEmail}";
        var deleted = await _service.DeleteAsync(id, profileId);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}