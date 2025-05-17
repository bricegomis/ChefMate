namespace ChefMate.API.Models.Documents;

public class StoreDocument : DocumentBase
{
    public required string Name { get; set; }
    public string? Image { get; set; }
}
