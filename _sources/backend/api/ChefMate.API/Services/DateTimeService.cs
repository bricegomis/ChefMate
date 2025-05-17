namespace ChefMate.API.Services;

public interface IDateTimeService
{
    public DateTimeOffset GetNow();
}

public class DateTimeService : IDateTimeService
{
    public DateTimeOffset GetNow()
    {
        return DateTimeOffset.UtcNow;
    }
}