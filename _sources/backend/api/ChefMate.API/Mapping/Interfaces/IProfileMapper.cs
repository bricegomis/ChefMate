using ChefMate.API.Models.Documents;
using ChefMate.API.Models.Dto;

namespace ChefMate.API.Mapping.Interfaces;

public interface IProfileMapper
{
    ProfileDto ToDto(ProfileDocument doc);
}