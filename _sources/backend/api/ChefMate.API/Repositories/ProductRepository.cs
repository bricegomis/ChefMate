using ChefMate.API.Models.Documents;
using ChefMate.API.Models.Documents.Interfaces;
using ChefMate.API.Services;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace ChefMate.API.Repositories;

public interface IProductRepository
{
    Task AddAsync(ProductDocument product);
    Task<ProductDocument> GetByIdAsync(string id);
    Task<List<ProductDocument>> GetAllAsync(string profileId);
    Task UpdateAsync(ProductDocument product);
    Task DeleteAsync(string id);
    Task BulkInsertAsync(IEnumerable<ProductDocument> products);
}

public class ProductRepository : IProductRepository
{
    private readonly IAsyncDocumentSession _session;
    private readonly IDocumentStore _store;
    private readonly IDateTimeService _dateTimeService;

    public ProductRepository(
        IAsyncDocumentSession session,
        IDocumentStore store,
        IDateTimeService dateTimeService)
    {
        _session = session;
        _store = store;
        _dateTimeService = dateTimeService;

        _store.OnBeforeStore += (sender, args) =>
        {
            if (args.Entity is IDateTracked dateTracked)
            {
                if (string.IsNullOrEmpty(args.DocumentId))
                {
                    dateTracked.DateCreated = _dateTimeService.GetNow();
                }
                dateTracked.DateModified = _dateTimeService.GetNow();
            }
        };
    }

    public async Task AddAsync(ProductDocument product)
    {
        await _session.StoreAsync(product);
        await _session.SaveChangesAsync();
    }

    public async Task<ProductDocument> GetByIdAsync(string id)
    {
        return await _session.LoadAsync<ProductDocument>(id);
    }

    public async Task<List<ProductDocument>> GetAllAsync(string profileId)
    {
        return await _session.Query<ProductDocument>()
            .Where(x => x.ProfileId == profileId)
            .ToListAsync();
    }

    public async Task UpdateAsync(ProductDocument product)
    {
        await _session.StoreAsync(product);
        await _session.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var product = await _session.LoadAsync<ProductDocument>(id);
        if (product != null)
        {
            _session.Delete(product);
            await _session.SaveChangesAsync();
        }
    }

    public async Task BulkInsertAsync(IEnumerable<ProductDocument> products)
    {       
        using var bulkInsert = _store.BulkInsert();
        foreach (var product in products)
        {
            await bulkInsert.StoreAsync(product);
        }
    }
}
