using ChefMate.Models;

namespace ChefMate.API.Manager;

public interface IProfileManager
{
    public Profile? CurrentProfile { get; }
    Task InitDefaultProfile();
}