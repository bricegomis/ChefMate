using ChefMate.API.Models.Enums;

namespace ChefMate.API.Models.Dto.Product;

public class ProductDto
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public List<string>? Labels { get; set; }
    public List<string>? Tags { get; set; }
    public List<PriceHistoryDto>? Prices { get; set; }
    public List<ProductUsageType>? Usages { get; set; }
    public DateTimeOffset DateCreated { get; set; }
    public DateTimeOffset DateModified { get; set; }
}