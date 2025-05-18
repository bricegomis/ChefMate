using ChefMate.API.Models.Documents;

namespace ChefMate.API.Models.Dto;

public class ProductDto
{
    public required string Id { get; set; }
    public required string ProfileId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public List<string>? Labels { get; set; }
    public List<string>? Tags { get; set; }
    public List<PriceHistory>? Prices { get; set; }
    public List<ProductUsageType>? Usages { get; set; }
    public DateTimeOffset DateCreated { get; set; }
    public DateTimeOffset DateModified { get; set; }
}