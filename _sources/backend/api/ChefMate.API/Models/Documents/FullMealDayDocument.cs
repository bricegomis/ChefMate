using ChefMate.API.Models.Documents.Interfaces;

namespace ChefMate.API.Models.Documents;

public class FullMealDayDocument :
    IProfileChild,
    IIdentifiable,
    IDateTracked
{
    public string? Id { get; set; }
    public required string ProfileId { get; set; }
    public DateTimeOffset DateCreated { get; set; }
    public DateTimeOffset DateModified { get; set; }

    public string? BreakfastRecipeId { get; set; }
    public string? LunchRecipeId { get; set; }
    public string? DinnerRecipeId { get; set; }
    public DateOnly Date { get; set; }

}