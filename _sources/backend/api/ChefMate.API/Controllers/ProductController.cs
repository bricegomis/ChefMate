using ChefMate.API.Models.Documents;
using ChefMate.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ChefMate.API.Models.Dto;

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
    public async Task<ActionResult<List<ProductDto>>> GetAll()
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(userEmail))
            return Unauthorized();

        var products = await _service.GetAllAsync(userEmail);
        // Mapping ProductDocument -> ProductDto
        var dtos = products.Select(p => new ProductDto
        {
            Id = p.Id,
            ProfileId = p.ProfileId,
            Name = p.Name,
            Labels = p.Labels,
            Type = p.Type,
            Comments = p.Comments,
            Tags = p.Tags,
            Image = p.Image,
            DateCreated = p.DateCreated,
            DateModified = p.DateModified
        }).ToList();

        return Ok(dtos);
    }
}