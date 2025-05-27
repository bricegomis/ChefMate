namespace ChefMate.API.Models.Dto.Store;

public class StoreUpdateDto
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public string? Image { get; set; }
}