﻿using ChefMate.API.Indexes;
using ChefMate.API.Models.Documents;

namespace ChefMate.API.Repositories.Interfaces;

public interface IProductRepository
{
    Task AddAsync(ProductDocument product);
    Task<ProductDocument> GetByIdAsync(string id);
    Task<List<ProductDocument>> GetAllAsync(string profileId);
    Task<List<ProductTagsIndex.Result>> GetTagsAsync(string profileId);
    Task UpdateAsync(ProductDocument product);
    Task DeleteAsync(string id);
}
