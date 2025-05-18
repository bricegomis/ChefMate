using ChefMate.API.Models.Documents;
using ChefMate.API.Models.Dto;
using Riok.Mapperly.Abstractions;
using ChefMate.API.Attributes;
using ChefMate.API.Mapping.Interfaces;

namespace ChefMate.API.Mapping;

[Injectable(ServiceLifetime.Singleton)]
[Mapper]
public partial class ProfileMapper : IProfileMapper
{
    public partial ProfileDto ToDto(ProfileDocument doc);
}