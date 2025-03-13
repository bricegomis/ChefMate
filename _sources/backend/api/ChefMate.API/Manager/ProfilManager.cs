using ChefMate.Models;
using ChefMate.API.Services;

namespace ChefMate.API.Manager;

public class ProfileManager(ILogger logger,
                           IMongoDBService mongoDBService,
                           IDateTimeProvider dateTimeProvider) : IProfileManager
{
    private readonly ILogger _logger = logger;
    private readonly IMongoDBService _mongoDBService = mongoDBService;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
    private readonly string _login = "test";

    public Profile? CurrentProfile { get; private set; }

    public async Task InitDefaultProfile()
    {
        _logger.LogInformation("========================> Starting init default profile");
        CurrentProfile = await _mongoDBService.GetProfile(_login);
        if (CurrentProfile == null)
        {
            _logger.LogInformation("========================> CurrentProfile null, creating new one");
            await _mongoDBService.CreateProfile(new Profile
            {
                Id = Guid.NewGuid().ToString(),
                Login = _login,
                DateCreated = _dateTimeProvider.GetNow(),
                DateLastConnection = _dateTimeProvider.GetNow()
            });
            CurrentProfile = await _mongoDBService.GetProfile(_login);
        }
        else
        {
            _logger.LogInformation("========================> CurrentProfile found, updating dateLastConnection");
            CurrentProfile.DateLastConnection = _dateTimeProvider.GetNow();
            await _mongoDBService.UpdateProfile(CurrentProfile);
        }

        _logger.LogInformation("========================> CurrentProfile init finished");
    }
}