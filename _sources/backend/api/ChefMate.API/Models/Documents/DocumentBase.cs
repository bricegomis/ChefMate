namespace ChefMate.API.Models.Documents;

public abstract class DocumentBase
{
    public string? Id { get; set; }
    public required string ProfileId { get; set; }
    public DateTimeOffset DateCreated { get; set; } = DateTimeOffset.Now;
    public DateTimeOffset DateModified { get; set; }
}
