using ChefMate.API.Attributes;

namespace ChefMate.API.Services;

public interface IDateTimeService
{
    public DateTimeOffset GetNow();
}

[Injectable]
public class DateTimeService : IDateTimeService
{
    public DateTimeOffset GetNow()
    {
        return DateTimeOffset.UtcNow;
    }
}