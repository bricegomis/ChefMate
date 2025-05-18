using ChefMate.API.Attributes;
using ChefMate.API.Models.Documents;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace ChefMate.API.Repositories;

[Injectable]
public class ProfileRepository(IAsyncDocumentSession session) : IProfileRepository
{
    public async Task<ProfileDocument?> GetByIdAsync(string id)
    {
        return await session.Query<ProfileDocument>()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ProfileDocument?> GetByEmailAsync(string id)
    {
        return await session.Query<ProfileDocument>()
            .FirstOrDefaultAsync(x => x.Email == id);
    }
}