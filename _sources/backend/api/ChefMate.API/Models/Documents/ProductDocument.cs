using ChefMate.API.Models.Documents.Interfaces;

namespace ChefMate.API.Models.Documents;

public class ProductDocument :
    IProfileChild,
    IIdentifiable,
    IDateTracked
{
    public required string Id { get; set; }
    public required string ProfileId { get; set; }
    public DateTimeOffset DateCreated { get; set; }
    public DateTimeOffset DateModified { get; set; }

    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    /// <summary>
    /// A/B, Bleu/Blanc/Coeur, Feroce, LabelRouge, etc.
    /// </summary>
    public List<string>? Labels { get; set; }
    public List<string>? Tags { get; set; }
    public List<PriceHistory>? Prices { get; set; }
    public List<ProductUsageType>? Usages { get; set; }
}

public class PriceHistory
{
    public required string StoreId { get; set; }
    public double Price { get; set; }
    public double Quantity { get; set; }
    public ProductQuantityUnit Unit { get; set; }
    public DateTimeOffset DateBuying { get; set; }
}

public enum ProductQuantityUnit
{
    Kg,
    L,
    Piece
}

public enum ProductUsageType
{
    Food,
    Cleaning,
    PersonalCare,
    Beverage,
    Supplement,
    Other
}