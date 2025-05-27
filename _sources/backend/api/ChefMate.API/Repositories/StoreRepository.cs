using ChefMate.API.Attributes;
using ChefMate.API.Models.Documents;
using ChefMate.API.Repositories.Interfaces;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace ChefMate.API.Repositories;

[Injectable]
public class StoreRepository(IAsyncDocumentSession session) : IStoreRepository
{
    public async Task AddAsync(StoreDocument store)
    {
        await session.StoreAsync(store);
        await session.SaveChangesAsync();
    }

    public async Task<StoreDocument> GetByIdAsync(string id)
    {
        return await session.LoadAsync<StoreDocument>(id);
    }

    public async Task<List<StoreDocument>> GetAllAsync(string profileId)
    {
        return await session.Query<StoreDocument>()
            .Where(x => x.ProfileId == profileId)
            .ToListAsync();
    }

    public async Task UpdateAsync(StoreDocument store)
    {
        await session.StoreAsync(store);
        await session.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var store = await session.LoadAsync<StoreDocument>(id);
        if (store != null)
        {
            session.Delete(store);
            await session.SaveChangesAsync();
        }
    }
}