using ChefMate.API.Attributes;
using ChefMate.API.Mapping.Interfaces;
using ChefMate.API.Models.Documents;
using ChefMate.API.Models.Dto.Product;
using ChefMate.API.Models.Dto.Store;
using Riok.Mapperly.Abstractions;

namespace ChefMate.API.Mapping;

[Injectable(ServiceLifetime.Singleton)]
[Mapper]
public partial class StoreMapper : IStoreMapper
{
    [MapperIgnoreSource(nameof(StoreDocument.ProfileId))]
    public partial StoreDto ToDto(StoreDocument doc);
    public partial List<StoreDto> ToDtoList(List<StoreDocument> docs);

    [MapperIgnoreTarget(nameof(StoreDocument.Id))]
    [MapperIgnoreTarget(nameof(StoreDocument.DateCreated))]
    [MapperIgnoreTarget(nameof(StoreDocument.DateModified))]
    public partial StoreDocument ToDocument(StoreCreateDto dto, string profileId);
    [MapperIgnoreTarget(nameof(StoreDocument.DateCreated))]
    [MapperIgnoreTarget(nameof(StoreDocument.DateModified))]
    public partial void UpdateDocument(StoreUpdateDto dto, StoreDocument document, string profileId);
}