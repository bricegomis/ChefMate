using ChefMate.API.Attributes;
using ChefMate.API.Models.Documents;
using ChefMate.API.Repositories.Interfaces;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace ChefMate.API.Repositories;

[Injectable]
public class ProductRepository(
    IAsyncDocumentSession session) : IProductRepository
{
    public async Task AddAsync(ProductDocument product)
    {
        await session.StoreAsync(product);
        await session.SaveChangesAsync();
    }

    public async Task<ProductDocument> GetByIdAsync(string id)
    {
        return await session.LoadAsync<ProductDocument>(id);
    }

    public async Task<List<ProductDocument>> GetAllAsync(string profileId)
    {
        return await session.Query<ProductDocument>()
            .Where(x => x.ProfileId == profileId)
            .ToListAsync();
    }

    public async Task<List<string>> GetTagsAsync(string profileId)
    {
        return await session.Query<ProductDocument>()
            .Where(x => x.ProfileId == profileId)
            .SelectMany(_ => _.Tags ?? new List<string>())
            .Distinct()
            .ToListAsync();
    }

    public async Task UpdateAsync(ProductDocument product)
    {
        await session.StoreAsync(product);
        await session.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var product = await session.LoadAsync<ProductDocument>(id);
        if (product != null)
        {
            session.Delete(product);
            await session.SaveChangesAsync();
        }
    }
}
