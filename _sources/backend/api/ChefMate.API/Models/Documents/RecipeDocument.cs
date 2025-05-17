namespace ChefMate.API.Models.Documents;

public class RecipeDocument : DocumentBase
{
    public string? Name { get; set; }
    public List<Ingredient> Ingredients { get; set; } = [];
    public string? Instructions { get; set; }
}

public class Ingredient
{
    public required string ProductId { get; set; }
    public required IngredientQuantity Quantity { get; set; }
}

public class IngredientQuantity
{
    public double Value { get; set; }
    public required IngredientUnit Unit { get; set; }
}

public enum IngredientUnit
{
    Gram,
    Milliliter,
    Piece,
    Tablespoon,
    Teaspoon
}
