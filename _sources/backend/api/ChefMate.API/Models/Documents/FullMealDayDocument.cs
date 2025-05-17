namespace ChefMate.API.Models.Documents;

public class FullMealDayDocument : DocumentBase
{
    public string? BreakfastRecipeId { get; set; }
    public string? LunchRecipeId { get; set; }
    public string? DinnerRecipeId { get; set; }
    public DateOnly Date { get; set; }
}