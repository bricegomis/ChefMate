using ChefMate.API.Models.Documents;
using ChefMate.API.Models.Dto.Store;

namespace ChefMate.API.Services.Interfaces;

public interface IStoreService
{
    Task<List<StoreDocument>> GetAllAsync(string profileId);
    Task<StoreDocument> GetByIdAsync(string id);
    Task<StoreDocument> AddAsync(StoreCreateDto dto, string profileId);
    Task<StoreDocument?> UpdateAsync(StoreUpdateDto dto, string profileId);
    Task DeleteAsync(string id);
    Task<bool> DeleteAsync(string id, string profileId);
}