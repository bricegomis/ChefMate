using ChefMate.API.Attributes;

namespace ChefMate.API.Services;

[Injectable]
public class DateTimeService : IDateTimeService
{
    public DateTimeOffset GetNow()
    {
        return DateTimeOffset.UtcNow;
    }
}