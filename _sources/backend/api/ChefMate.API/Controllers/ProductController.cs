using ChefMate.API.Models.Documents;
using ChefMate.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ChefMate.API.Models.Dto;
using ChefMate.API.Mapping;

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

    [HttpPost("import")]
    public async Task<IActionResult> Import([FromBody] List<ProductDocument> products)
    {
        await _service.BulkImportAsync(products);
        return Ok(new { Message = "Products imported", products.Count });
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> GetAll()
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(userEmail))
            return Unauthorized();

        var products = await _service.GetAllAsync(userEmail);
        var dtos = _mapper.ToDtoList(products);
        return Ok(dtos);
    }
}