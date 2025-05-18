using ChefMate.API.Models.Enums;

namespace ChefMate.API.Models.Dto.Product;

public class PriceHistoryDto
{
    public double Price { get; set; }
    public double Quantity { get; set; }
    public ProductQuantityUnit Unit { get; set; }
    public DateTimeOffset DateBuying { get; set; }
    public required string StoreId { get; set; }
}