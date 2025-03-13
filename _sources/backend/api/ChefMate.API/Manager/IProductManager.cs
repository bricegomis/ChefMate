using ChefMate.Models;

namespace ChefMate.API.Manager;

public interface IProductManager
{
    Task<Product> GetProduct(string id);
    Task<List<Product>> GetAllProducts();
    //Task FinishProduct(Product product);
    Task UpdateProduct(Product product);
    //Task DeleteProduct(string id);
    Task CreateProduct(Product product);
    Task CustomMethod();
}