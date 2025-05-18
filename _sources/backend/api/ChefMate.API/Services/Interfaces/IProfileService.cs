using ChefMate.API.Models.Dto;
using System.Threading.Tasks;

namespace ChefMate.API.Services.Interfaces;

public interface IProfileService
{
    Task<ProfileDto?> GetCurrentProfileAsync(string profileId);
    Task<ProfileDto?> GetProfileFromEmailAsync(string email);
}