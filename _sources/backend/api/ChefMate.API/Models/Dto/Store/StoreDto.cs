namespace ChefMate.API.Models.Dto.Store;

public class StoreDto
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public string? Image { get; set; }
    public DateTimeOffset DateCreated { get; set; }
    public DateTimeOffset DateModified { get; set; }
}