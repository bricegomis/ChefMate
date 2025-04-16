using ChefMate.API.Manager;
using ChefMate.API.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Diagnostics;

namespace ChefMate.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController(ILogger<ProductController> logger,
                        IProductManager productManager) : ControllerBase
{
    private readonly ILogger<ProductController> _logger = logger;
    private readonly IProductManager _productManager = productManager;

    [HttpGet("CustomMethod", Name = "CustomMethod")]
    [Produces(typeof(IEnumerable<Product>))]
    [SwaggerOperation(OperationId = "CustomMethod")]
    public async Task CustomMethod()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        _logger.LogDebug("CustomMethod start");
        await _productManager.CustomMethod();

        stopwatch.Stop();
        _logger.LogDebug("CustomMethod: {stopwatch.ElapsedMilliseconds} ms", stopwatch.ElapsedMilliseconds);
    }

    [HttpGet("all", Name = "GetAll")]
    [Produces(typeof(IEnumerable<Product>))]
    [SwaggerOperation(OperationId = "GetAll")]
    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        _logger.LogDebug("GetProductsAsync start");
        var products = await _productManager.GetAllProducts();

        stopwatch.Stop();
        _logger.LogDebug("GetproductAsync: {stopwatch.ElapsedMilliseconds} ms", stopwatch.ElapsedMilliseconds);

        return products;
    }

    [HttpGet("{id}", Name = "GetById")]
    [Produces(typeof(Product))]
    [SwaggerResponse((int)System.Net.HttpStatusCode.OK, Type = typeof(Product))]
    public async Task<Product> GetProduct(string id)
    {
        _logger.LogDebug("GetProduct called with id: {id}", id);
        return await _productManager.GetProduct(id);
    }

    [HttpPost(Name = "Create")]
    [SwaggerResponse((int)System.Net.HttpStatusCode.OK)]
    public async Task CreateProduct(Product item)
    {
        _logger.LogDebug("CreateProduct called with id: {id}", item?.Id ?? "null");
        if (item == null) return;
        await _productManager.CreateProduct(item);
    }

    [HttpPut(Name = "Update")]
    [SwaggerResponse((int)System.Net.HttpStatusCode.OK)]
    public async Task UpdateProduct(Product item)
    {
        _logger.LogDebug("UpdateProduct called with id: {id}", item?.Id ?? "null");
        if (item == null) return;
        await _productManager.UpdateProduct(item);
    }

    [HttpDelete("{id}", Name = "Delete")]
    [SwaggerResponse((int)System.Net.HttpStatusCode.OK)]
    public async Task DeleteProduct(string id)
    {
        _logger.LogDebug("DeleteProduct called with id: {id}", id ?? "null");
        if (id == null) return;
        await _productManager.DeleteProduct(id);
    }
}