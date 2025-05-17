using ChefMate.API.Models.Documents;
using ChefMate.API.Repositories;

namespace ChefMate.API.Services;

public interface IProductService
{
    Task BulkImportAsync(IEnumerable<ProductDocument> products);
    Task<List<ProductDocument>> GetAllAsync(string profileId);
    Task<ProductDocument> GetByIdAsync(string id);
    Task AddAsync(ProductDocument product);
    Task UpdateAsync(ProductDocument product);
    Task DeleteAsync(string id);
}

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public Task BulkImportAsync(IEnumerable<ProductDocument> products)
    {
        return _repository.BulkInsertAsync(products);
    }

    public Task<List<ProductDocument>> GetAllAsync(string profileId)
    {
        return _repository.GetAllAsync(profileId);
    }

    public Task<ProductDocument> GetByIdAsync(string id)
    {
        return _repository.GetByIdAsync(id);
    }

    public Task AddAsync(ProductDocument product)
    {
        product.DateCreated = DateTimeOffset.UtcNow;
        product.DateModified = DateTimeOffset.UtcNow;
        return _repository.AddAsync(product);
    }

    public Task UpdateAsync(ProductDocument product)
    {
        product.DateModified = DateTimeOffset.UtcNow;
        return _repository.UpdateAsync(product);
    }

    public Task DeleteAsync(string id)
    {
        return _repository.DeleteAsync(id);
    }
}

