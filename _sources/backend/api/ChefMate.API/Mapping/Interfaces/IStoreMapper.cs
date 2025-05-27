using ChefMate.API.Models.Documents;
using ChefMate.API.Models.Dto.Store;

namespace ChefMate.API.Mapping.Interfaces;

public interface IStoreMapper
{
    StoreDto ToDto(StoreDocument doc);
    List<StoreDto> ToDtoList(List<StoreDocument> docs);
    StoreDocument ToDocument(StoreCreateDto dto, string profileId);
    void UpdateDocument(StoreUpdateDto dto, StoreDocument document, string profileId);
}