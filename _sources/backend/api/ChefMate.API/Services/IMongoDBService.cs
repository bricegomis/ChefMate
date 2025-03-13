using ChefMate.Models;

namespace ChefMate.API.Services;

public interface IMongoDBService
{
    Task CustomMethod();
    Task<List<Product>> GetAllProducts(string profileId);
    Task<Product> GetProduct(string id);
    Task CreateProduct(Product product);
    Task UpdateProduct(Product product);
    Task DeleteProduct(string id);

    Task<Profile> GetProfile(string login);
    Task CreateProfile(Profile profile);
    Task<bool> UpdateProfile(Profile profile);
    Task<bool> DeleteProfile(Profile profile);
}