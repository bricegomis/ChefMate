using MongoDB.Bson.Serialization.Attributes;

namespace ChefMate.Models;

[BsonIgnoreExtraElements]
public class PriceItem : ModelBase
{
    public required string StoreName { get; set; }
    public double Price { get; set; }
    public DateTimeOffset DateBuying { get; set; }
}