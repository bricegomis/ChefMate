using MongoDB.Bson.Serialization.Attributes;

namespace ChefMate.Models;

[BsonIgnoreExtraElements]
public class Profile : ModelBase
{
    public required string Login { get; set; }
    public DateTimeOffset DateLastConnection { get; set; }
}
