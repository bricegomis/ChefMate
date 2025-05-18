using ChefMate.API.Attributes;
using ChefMate.API.Services.Interfaces;

namespace ChefMate.API.Services;

[Injectable(ServiceLifetime.Singleton)]
public class DateTimeService : IDateTimeService
{
    public DateTimeOffset GetNow()
    {
        return DateTimeOffset.UtcNow;
    }
}