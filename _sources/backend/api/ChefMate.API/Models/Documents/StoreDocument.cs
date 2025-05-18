using ChefMate.API.Models.Documents.Interfaces;

namespace ChefMate.API.Models.Documents;

public class StoreDocument :
    IProfileChild,
    IIdentifiable,
    IDateTracked
{
    public required string Id { get; set; }
    public required string ProfileId { get; set; }
    public DateTimeOffset DateCreated { get; set; }
    public DateTimeOffset DateModified { get; set; }

    public required string Name { get; set; }
    public string? Image { get; set; }
}
