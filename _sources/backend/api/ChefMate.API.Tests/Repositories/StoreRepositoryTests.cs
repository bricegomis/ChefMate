using ChefMate.API.Models.Documents;
using ChefMate.API.Repositories;
using FluentAssertions;
using Moq;
using Raven.Client.Documents.Session;

namespace ChefMate.API.Tests.Repositories;

public class StoreRepositoryTests
{
    private readonly Mock<IAsyncDocumentSession> _sessionMock = new();

    private StoreRepository CreateRepo()
        => new(_sessionMock.Object);

    [Fact]
    public async Task AddAsync_Stores_And_Saves()
    {
        var repo = CreateRepo();
        var store = new StoreDocument
        {
            Id = "id1",
            ProfileId = "profile1",
            Name = "S"
        };

        await repo.AddAsync(store);

        _sessionMock.Verify(s => s.StoreAsync(store, default), Times.Once);
        _sessionMock.Verify(s => s.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_Loads_Store()
    {
        var repo = CreateRepo();
        var store = new StoreDocument
        {
            Id = "id1",
            ProfileId = "profile1",
            Name = "S"
        };
        _sessionMock.Setup(s => s.LoadAsync<StoreDocument>("id1", default)).ReturnsAsync(store);

        var result = await repo.GetByIdAsync("id1");

        result.Should().Be(store);
    }

    [Fact]
    public async Task UpdateAsync_Stores_And_Saves()
    {
        var repo = CreateRepo();
        var store = new StoreDocument
        {
            Id = "id1",
            ProfileId = "profile1",
            Name = "S"
        };

        await repo.UpdateAsync(store);

        _sessionMock.Verify(s => s.StoreAsync(store, default), Times.Once);
        _sessionMock.Verify(s => s.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_Deletes_And_Saves_When_Exists()
    {
        var repo = CreateRepo();
        var store = new StoreDocument
        {
            Id = "id1",
            ProfileId = "profile1",
            Name = "S"
        };
        _sessionMock.Setup(s => s.LoadAsync<StoreDocument>("id1", default)).ReturnsAsync(store);

        await repo.DeleteAsync("id1");

        _sessionMock.Verify(s => s.Delete(store), Times.Once);
        _sessionMock.Verify(s => s.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_DoesNothing_When_NotFound()
    {
        var repo = CreateRepo();
        _sessionMock.Setup(s => s.LoadAsync<StoreDocument>("id1", default)).ReturnsAsync((StoreDocument?)null);

        await repo.DeleteAsync("id1");

        _sessionMock.Verify(s => s.Delete(It.IsAny<StoreDocument>()), Times.Never);
        _sessionMock.Verify(s => s.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}