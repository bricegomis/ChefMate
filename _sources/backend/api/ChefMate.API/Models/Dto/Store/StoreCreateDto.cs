namespace ChefMate.API.Models.Dto.Store;

public class StoreCreateDto
{
    public required string Name { get; set; }
    public string? Image { get; set; }
}