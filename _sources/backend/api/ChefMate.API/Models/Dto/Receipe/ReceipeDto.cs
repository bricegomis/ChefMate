using ChefMate.API.Models.Enums;

namespace ChefMate.API.Models.Dto.Receipe;

public class ReceipeDto
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public List<IngredientDto>? Ingredients { get; set; }
    public string? Instructions { get; set; }
}

public class IngredientDto
{
    public required string ProductId { get; set; }
    public required double Value { get; set; }
    public required IngredientUnit Unit { get; set; }
}
