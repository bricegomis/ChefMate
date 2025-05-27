using ChefMate.API.Attributes;
using ChefMate.API.Mapping.Interfaces;
using ChefMate.API.Models.Documents;
using ChefMate.API.Models.Dto.Store;
using ChefMate.API.Repositories.Interfaces;
using ChefMate.API.Services.Interfaces;

namespace ChefMate.API.Services;

[Injectable]
public class StoreService(IStoreRepository repository, IStoreMapper mapper) : IStoreService
{
    private readonly IStoreRepository _repository = repository;
    private readonly IStoreMapper _mapper = mapper;

    public Task<List<StoreDocument>> GetAllAsync(string profileId)
    {
        return _repository.GetAllAsync(profileId);
    }

    public Task<StoreDocument> GetByIdAsync(string id)
    {
        return _repository.GetByIdAsync(id);
    }

    public async Task<StoreDocument> AddAsync(StoreCreateDto dto, string profileId)
    {
        var store = _mapper.ToDocument(dto, profileId);
        await _repository.AddAsync(store);
        return store;
    }

    public async Task<StoreDocument?> UpdateAsync(StoreUpdateDto dto, string profileId)
    {
        var store = await _repository.GetByIdAsync(dto.Id);
        if (store.ProfileId != profileId)
            throw new ApplicationException("Profile mismatch.");

        _mapper.UpdateDocument(dto, store, profileId);
        await _repository.UpdateAsync(store);
        return store;
    }

    public Task DeleteAsync(string id)
    {
        return _repository.DeleteAsync(id);
    }

    public async Task<bool> DeleteAsync(string id, string profileId)
    {
        var store = await _repository.GetByIdAsync(id);
        if (store.ProfileId != profileId)
            throw new ApplicationException("Profile mismatch.");

        await _repository.DeleteAsync(id);
        return true;
    }
}