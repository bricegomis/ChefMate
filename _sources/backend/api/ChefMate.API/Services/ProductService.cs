using ChefMate.API.Attributes;
using ChefMate.API.Mapping.Interfaces;
using ChefMate.API.Models.Documents;
using ChefMate.API.Models.Dto.Product;
using ChefMate.API.Repositories;
using ChefMate.API.Services.Interfaces;

namespace ChefMate.API.Services;

[Injectable]
public class ProductService(IProductRepository repository, IProductMapper mapper) : IProductService
{
    private readonly IProductRepository _repository = repository;
    private readonly IProductMapper _mapper = mapper;

    public Task<List<ProductDocument>> GetAllAsync(string profileId)
    {
        return _repository.GetAllAsync(profileId);
    }

    public Task<List<string>> GetTagsAsync(string profileId)
    {
        return _repository.GetTagsAsync(profileId);
    }

    public Task<ProductDocument> GetByIdAsync(string id)
    {
        return _repository.GetByIdAsync(id);
    }

    public async Task<ProductDocument> AddAsync(ProductCreateDto dto, string profileId)
    {
        var product = _mapper.ToDocument(dto, profileId);
        await _repository.AddAsync(product);
        return product;
    }

    public async Task<ProductDocument?> UpdateAsync(ProductUpdateDto dto, string profileId)
    {
        var product = await _repository.GetByIdAsync(dto.Id);
        if (product.ProfileId != profileId)
            throw new ApplicationException("Profile mismatch.");

        _mapper.UpdateDocument(dto, product, profileId);
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
        if (product.ProfileId != profileId)
            throw new ApplicationException("Profile mismatch.");

        await _repository.DeleteAsync(id);
        return true;
    }
}

