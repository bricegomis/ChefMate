using ChefMate.API.Models.Documents;
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

    [HttpPost("import")]
    public async Task<IActionResult> Import([FromBody] List<ProductDocument> products)
    {
        await _service.BulkImportAsync(products);
        return Ok(new { Message = "Products imported", products.Count });
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductDocument>>> GetAll()
    {
        var profileId = $"profiles/{User.FindFirst(ClaimTypes.Email)?.Value}";
        var products = await _service.GetAllAsync(profileId);
        return Ok(products);
    }
    }
}