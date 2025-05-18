using ChefMate.API.Mapping;
using ChefMate.API.Models.Dto;
using ChefMate.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChefMate.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProductController(
    ILogger<ProductController> logger,
    IProductService productService,
    IProfileContextService profileContext) : ControllerBase
{
    private readonly ILogger<ProductController> _logger = logger;
    private readonly IProductService _service = productService;
    private readonly IProfileContextService _profileContext = profileContext;
    private static readonly ProductMapper _mapper = new();

    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> GetAll()
    {
        var profileId = _profileContext.GetCurrentProfileId(User);
        var products = await _service.GetAllAsync(profileId);
        var dtos = _mapper.ToDtoList(products);
        return Ok(dtos);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create([FromBody] ProductCreateDto dto)
    {
        var profileId = _profileContext.GetCurrentProfileId(User);
        var product = await _service.AddAsync(dto, profileId);
        return CreatedAtAction(nameof(GetAll), new { id = product.Id }, _mapper.ToDto(product));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] ProductUpdateDto dto)
    {
        if (id != dto.Id)
            return BadRequest("Id mismatch");

        var profileId = _profileContext.GetCurrentProfileId(User);
        var updated = await _service.UpdateAsync(dto, profileId);
        if (updated == null)
            return NotFound();

        return Ok(_mapper.ToDto(updated));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var profileId = _profileContext.GetCurrentProfileId(User);
        var deleted = await _service.DeleteAsync(id, profileId);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}