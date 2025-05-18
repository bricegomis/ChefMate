using System.Security.Claims;

namespace ChefMate.API.Services.Interfaces;

public interface IProfileContextService
{
    /// <summary>
    /// R�cup�re le profileId du ClaimsPrincipal courant (ex: "profiles/user@email.com").
    /// </summary>
    string GetCurrentProfileId(ClaimsPrincipal user);
}