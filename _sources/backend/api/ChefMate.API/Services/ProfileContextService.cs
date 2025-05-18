using System.Security.Claims;
using ChefMate.API.Attributes;
using ChefMate.API.Services.Interfaces;

namespace ChefMate.API.Services;

[Injectable]
public class ProfileContextService : IProfileContextService
{
    public string GetCurrentProfileId(ClaimsPrincipal user)
    {
        var email = user.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(email))
            throw new InvalidOperationException("No email claim found for authenticated user.");
        return $"profiles/{email}";
    }
}