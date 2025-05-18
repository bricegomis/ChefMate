using ChefMate.API.Attributes;
using ChefMate.API.Models.Dto;
using ChefMate.API.Repositories;
using ChefMate.API.Mapping;
using ChefMate.API.Mapping.Interfaces;
using ChefMate.API.Services.Interfaces;

namespace ChefMate.API.Services;

[Injectable]
public class ProfileService(IProfileRepository repo, IProfileMapper mapper) : IProfileService
{
    private readonly IProfileRepository _repo = repo;
    private readonly IProfileMapper _mapper = mapper;

    public async Task<ProfileDto?> GetCurrentProfileAsync(string profileId)
    {
        var doc = await _repo.GetByIdAsync(profileId);
        return doc is null ? null : _mapper.ToDto(doc);
    }

    public async Task<ProfileDto?> GetProfileFromEmailAsync(string email)
    {
        var doc = await _repo.GetByEmailAsync(email);
        return doc is null ? null : _mapper.ToDto(doc);
    }
}