using ChefMate.API.Models.Documents;
using ChefMate.API.Models.Dto.Product;

namespace ChefMate.API.Services.Interfaces;

public interface IProductService
{
    Task<List<ProductDocument>> GetAllAsync(string profileId);
    Task<ProductDocument> GetByIdAsync(string id);
    Task DeleteAsync(string id);
    Task<ProductDocument> AddAsync(ProductCreateDto dto, string profileId);
    Task<ProductDocument?> UpdateAsync(ProductUpdateDto dto, string profileId);
    Task<bool> DeleteAsync(string id, string profileId);
}

