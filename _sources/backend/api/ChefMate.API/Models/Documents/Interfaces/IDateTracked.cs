namespace ChefMate.API.Models.Documents.Interfaces;

public interface IDateTracked
{
    DateTimeOffset DateCreated { get; set; }
    DateTimeOffset DateModified { get; set; }
}