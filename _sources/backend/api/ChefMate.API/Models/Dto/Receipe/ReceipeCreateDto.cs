namespace ChefMate.API.Models.Dto.Receipe;

public class ReceipeCreateDto
{
    public string? Name { get; set; }
    public List<IngredientDto>? Ingredients { get; set; }
    public string? Instructions { get; set; }
}