using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChefMate.Models;

[BsonIgnoreExtraElements]
public class Product : ModelBase
{
    public required string Name { get; set; }
    public List<string>? Labels { get; set; }
    public string? Type { get; set; }
    public string? Comments { get; set; }
    public string? Unit { get; set; }
    public List<PriceItem>? Prices { get; set; }
    public List<string>? Tags { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string? ProfileId { get; set; }
}