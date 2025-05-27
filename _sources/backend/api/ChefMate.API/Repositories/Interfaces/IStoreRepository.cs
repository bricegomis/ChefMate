using ChefMate.API.Models.Documents;

namespace ChefMate.API.Repositories.Interfaces;

public interface IStoreRepository
{
    Task AddAsync(StoreDocument store);
    Task<StoreDocument> GetByIdAsync(string id);
    Task<List<StoreDocument>> GetAllAsync(string profileId);
    Task UpdateAsync(StoreDocument store);
    Task DeleteAsync(string id);
}
