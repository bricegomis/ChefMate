namespace ChefMate.API.Services;

public interface IDateTimeProvider
{
    public DateTimeOffset GetNow();
}
