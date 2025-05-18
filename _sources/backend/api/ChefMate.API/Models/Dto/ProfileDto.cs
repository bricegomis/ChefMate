namespace ChefMate.API.Models.Dto;

public class ProfileDto
{
    public required string Id { get; set; }
    public required string Email { get; set; }
    public string? DisplayName { get; set; }
    public string? AvatarUrl { get; set; }
    public DateTimeOffset DateCreated { get; set; }
    public DateTimeOffset DateModified { get; set; }
}