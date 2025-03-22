namespace ChefMate.API.Manager;

using ChefMate.Models;
using ChefMate.API.Services;
using MongoDB.Bson;
using MongoDB.Driver;

public class ProductManager(ILogger logger,
                               IMongoDBService mongoDBService,
                               IProfileManager profileManager,
                               IDateTimeProvider dateTimeProvider) : IProductManager
{
    private const string _login = "Test";
    private readonly IMongoDBService _mongoDBService = mongoDBService;
    private readonly IProfileManager _profileManager = profileManager;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
    private readonly ILogger _logger = logger;
    
    //public async Task FinishProduct(Product productFrom)
    //{
    //    if (productFrom is null || productFrom.Id is null)
    //    {
    //        _logger.LogError("product is null");
    //        return;
    //    }
    //    if (_profileManager.CurrentProfile is null)
    //    {
    //        _logger.LogError("CurrentProfile is null");
    //        return;
    //    }
    //    var product = await _mongoDBService.GetProduct(productFrom.Id);
    //    if (product is null)
    //    {
    //        _logger.LogError($"product not found with id {productFrom.Id}");
    //        return;
    //    }

    //    // Force the profilId to ensure it's not update from the client
    //    product.ProfileId = _profileManager.CurrentProfile.Id;
    //    product.DateModified = _dateTimeProvider.GetNow();

    //    if (product.History is null)
    //        product.History = [];
    //    product.History.Add(_dateTimeProvider.GetNow());
    //    product.HistoryBonus += 1;// TODO: reduce if bad ?

    //    if (product.Points.HasValue) {
    //        var pts = product.IsReward == true ? product.Points.Value : -product.Points.Value;
    //        _profileManager.CurrentProfile.ScoreTotal += pts;
    //        _profileManager.CurrentProfile.ScoreWeek += pts;
    //        _profileManager.CurrentProfile.ScoreDay += pts;
    //    }

    //    await _mongoDBService.UpdateProduct(product);
    //    await _mongoDBService.UpdateProfile(_profileManager.CurrentProfile);

    //    var history = new HistoryItem
    //    {
    //        Item = product,
    //        Date = _dateTimeProvider.GetNow(),
    //        Points = product.IsReward == true ? product.Points ?? 0 : -product.Points ?? 0,
    //        ProfileId = _profileManager.CurrentProfile.Id
    //    };
    //    await _mongoDBService.CreateHistory(history);
    //}

    public async Task UpdateProduct(Product product)
    {
        if (product is null)
        {
            _logger.LogError("product is null");
            return;
        }
        if (_profileManager.CurrentProfile is null)
        {
            _logger.LogError("_profileManager.CurrentProfile is null");
            return;
        }

        // Force the profilId to ensure it's not update from the client
        product.ProfileId = _profileManager.CurrentProfile.Id;
        product.DateModified = _dateTimeProvider.GetNow();
        await _mongoDBService.UpdateProduct(product);
    }

    public async Task DeleteProduct(string id)
    {
        if (_profileManager.CurrentProfile is null) return;
        // Check the product online to be sure
        var product = await _mongoDBService.GetProduct(id);
        if (product is null || product.Id is null) return;
        if (product.ProfileId != _profileManager.CurrentProfile.Id)
        {
            _logger.LogWarning("Try to delete a product with different profileId than currentProfile");
            return;
        }

        await _mongoDBService.DeleteProduct(product.Id);
    }

    public async Task CreateProduct(Product product)
    {
        _logger.LogInformation("CreateProduct");
        if (_profileManager.CurrentProfile == null || product is null
            || product.Id is null)
        {
            _logger.LogError("_profileManager.CurrentProfile not defined");
            return;
        }
        product.ProfileId = _profileManager.CurrentProfile.Id;
        product.DateCreated = _dateTimeProvider.GetNow();
        product.DateModified = _dateTimeProvider.GetNow();
        await _mongoDBService.CreateProduct(product);

        await _mongoDBService.UpdateProfile(_profileManager.CurrentProfile);
    }

    public async Task<Product> GetProduct(string id)
    {
        return await _mongoDBService.GetProduct(id);
    }

    public async Task<List<Product>> GetAllProducts()
    {
        if (_profileManager.CurrentProfile is null || _profileManager.CurrentProfile.Id is null) return [];
        return await _mongoDBService.GetAllProducts(_profileManager.CurrentProfile.Id);
    }

    public async Task CustomMethod()
    {
        await _mongoDBService.CustomMethod();
    }
}
