using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChefMate.API.Models;

public abstract class ModelBase
{
    [BsonId]
    public required string Id { get; set; }
    public DateTimeOffset DateCreated { get; set; }
    public DateTimeOffset DateModified { get; set; }
    public DateTimeOffset DateDeleted { get; set; }
    public bool? IsDeleted { get; set; }
}
