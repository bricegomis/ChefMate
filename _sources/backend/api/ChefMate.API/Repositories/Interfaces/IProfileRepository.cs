using ChefMate.API.Models.Documents;
using System.Threading.Tasks;

namespace ChefMate.API.Repositories.Interfaces;

public interface IProfileRepository
{
    Task<ProfileDocument?> GetByIdAsync(string id);
    Task<ProfileDocument?> GetByEmailAsync(string email);
}