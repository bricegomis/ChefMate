using ChefMate.Models;
using MongoDB.Driver;

namespace ChefMate.API.Services;

public class MongoDBService : IMongoDBService
{
    private readonly IMongoCollection<Product> _productCollection;
    private readonly IMongoCollection<Profile> _profileCollection;
    private readonly MongoClient _client;

    private readonly ILogger _logger;
    private readonly IDateTimeProvider _dateTimeProvider;

    public MongoDBService(
        ILogger logger,
        IDateTimeProvider dateTimeProvider,
        string connectionString,
        string dbName
    )
    {
        _logger = logger;
        _dateTimeProvider = dateTimeProvider;
        _client = new MongoClient(connectionString);

        string productCollectionName = "Ingredients";
        string profileCollectionName = "Profiles";

        var database = _client.GetDatabase(dbName);
        _productCollection = database.GetCollection<Product>(productCollectionName);
        _profileCollection = database.GetCollection<Profile>(profileCollectionName);
    }

    public async Task CustomMethod()
    {
        var updateDefinition = Builders<Product>.Update.Set(product => product.Labels, ["Bio"]);

        var result = await _productCollection.UpdateManyAsync(
            filter: Builders<Product>.Filter.Empty,
            update: updateDefinition
        );
    }

    public async Task<List<Product>> GetAllProducts(string profileId)
    {
        var productCursor = await _productCollection.FindAsync(_ => _.ProfileId == profileId);
        var products = await productCursor.ToListAsync();
        return products;
    }

    public async Task<Product> GetProduct(string id)
    {
        return await _productCollection.Find(x => x.Id == id).FirstAsync();
    }

    public async Task CreateProduct(Product product)
    {
        product.DateCreated = _dateTimeProvider.GetNow();
        product.DateModified = _dateTimeProvider.GetNow();
        await _productCollection.InsertOneAsync(product);
    }

    public async Task UpdateProduct(Product product)
    {
        product.DateModified = _dateTimeProvider.GetNow();
        await _productCollection.ReplaceOneAsync(_ => _.Id == product.Id, product);
    }

    public async Task DeleteProduct(string id)
    {
        await _productCollection.DeleteOneAsync(x => x.Id == id);
    }

    #region Profiles
    public async Task CreateProfile(Profile profile)
    {
        await _profileCollection.InsertOneAsync(profile);
    }

    public async Task<Profile> GetProfile(string login)
    {
        _logger.LogDebug($"GetProfil : {login}");
        return await _profileCollection.Find(x => x.Login == login).FirstOrDefaultAsync();
    }

    public async Task<bool> UpdateProfile(Profile profile)
    {
        var filter = Builders<Profile>.Filter.Eq(_ => _.Id, profile.Id);
        var result = await _profileCollection.ReplaceOneAsync(filter, profile);
        return result.IsAcknowledged && result.MatchedCount == 1;
    }

    public async Task<bool> DeleteProfile(Profile profile)
    {
        var filter = Builders<Profile>.Filter.Eq(_ => _.Id, profile.Id);
        var result = await _profileCollection.DeleteOneAsync(filter);
        return result.IsAcknowledged && result.DeletedCount == 1;
    }
    #endregion
}
