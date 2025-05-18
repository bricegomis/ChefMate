﻿using ChefMate.API.Models.Documents.Interfaces;
using ChefMate.API.Models.Enums;

namespace ChefMate.API.Models.Documents;

public class RecipeDocument :
    IProfileChild,
    IIdentifiable,
    IDateTracked
{
    public string? Id { get; set; }
    public required string ProfileId { get; set; }
    public DateTimeOffset DateCreated { get; set; }
    public DateTimeOffset DateModified { get; set; }

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
