using ChefMate.API.Attributes;
using ChefMate.API.Services.Interfaces;

namespace ChefMate.API.Services;

[Injectable]
public class DateTimeService : IDateTimeService
{
    public DateTimeOffset GetNow()
    {
        return DateTimeOffset.UtcNow;
    }
}