using ChefMate.API.Attributes;
using ChefMate.API.Mapping;
using ChefMate.API.Models.Documents;
using ChefMate.API.Models.Dto;
using ChefMate.API.Repositories;

namespace ChefMate.API.Services;

[Injectable]
public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private static readonly ProductMapper _mapper = new();

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

    public async Task<ProductDocument> AddAsync(ProductCreateDto dto, string profileId)
    {
        var product = _mapper.ToDocument(dto, profileId);
        product.DateCreated = DateTimeOffset.UtcNow;
        product.DateModified = DateTimeOffset.UtcNow;
        await _repository.AddAsync(product);
        return product;
    }

    public async Task<ProductDocument?> UpdateAsync(ProductUpdateDto dto, string profileId)
    {
        var product = await _repository.GetByIdAsync(dto.Id);
        if (product == null || product.ProfileId != profileId)
            return null;

        _mapper.UpdateDocument(dto, product, profileId);
        product.DateModified = DateTimeOffset.UtcNow;
        await _repository.UpdateAsync(product);
        return product;
    }

    public Task DeleteAsync(string id)
    {
        return _repository.DeleteAsync(id);
    }

    public async Task<bool> DeleteAsync(string id, string profileId)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null || product.ProfileId != profileId)
            return false;

        await _repository.DeleteAsync(id);
        return true;
    }
}

