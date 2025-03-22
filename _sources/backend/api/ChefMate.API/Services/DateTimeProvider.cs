namespace ChefMate.API.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset GetNow()
    {
        return DateTimeOffset.UtcNow;
    }
}
