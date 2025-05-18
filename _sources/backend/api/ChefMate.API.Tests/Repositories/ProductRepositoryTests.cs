using ChefMate.API.Models.Documents;
using ChefMate.API.Repositories;
using ChefMate.API.Services.Interfaces;
using FluentAssertions;
using Moq;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;

namespace ChefMate.API.Tests.Repositories;

public class ProductRepositoryTests
{
    private readonly Mock<IAsyncDocumentSession> _sessionMock = new();
    private readonly Mock<IDocumentStore> _storeMock = new();
    private readonly Mock<IDateTimeService> _dateTimeServiceMock = new();

    private ProductRepository CreateRepo()
        => new(_sessionMock.Object, _storeMock.Object, _dateTimeServiceMock.Object);

    [Fact]
    public async Task AddAsync_Stores_And_Saves()
    {
        var repo = CreateRepo();
        var product = new ProductDocument
        {
            Id = "id1",
            ProfileId = "profile1",
            Name = "P"
        };

        await repo.AddAsync(product);

        _sessionMock.Verify(s => s.StoreAsync(product, CancellationToken.None), Times.Once);
        _sessionMock.Verify(s => s.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_Loads_Product()
    {
        var repo = CreateRepo();
        var product = new ProductDocument
        {
            Id = "id1",
            ProfileId = "profile1",
            Name = "P"
        };
        _sessionMock.Setup(s => s.LoadAsync<ProductDocument>("id1", default)).ReturnsAsync(product);

        var result = await repo.GetByIdAsync("id1");

        result.Should().Be(product);
    }

    [Fact]
    public async Task UpdateAsync_Stores_And_Saves()
    {
        var repo = CreateRepo();
        var product = new ProductDocument
        {
            Id = "id1",
            ProfileId = "profile1",
            Name = "P"
        };

        await repo.UpdateAsync(product);

        _sessionMock.Verify(s => s.StoreAsync(product, default), Times.Once);
        _sessionMock.Verify(s => s.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_Deletes_And_Saves_When_Exists()
    {
        var repo = CreateRepo();
        var product = new ProductDocument
        {
            Id = "id1",
            ProfileId = "profile1",
            Name = "P"
        };
        _sessionMock.Setup(s => s.LoadAsync<ProductDocument>("id1", default)).ReturnsAsync(product);

        await repo.DeleteAsync("id1");

        _sessionMock.Verify(s => s.Delete(product), Times.Once);
        _sessionMock.Verify(s => s.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_DoesNothing_When_NotFound()
    {
        var repo = CreateRepo();
        _sessionMock.Setup(s => s.LoadAsync<ProductDocument>("id1", default)).ReturnsAsync((ProductDocument?)null);

        await repo.DeleteAsync("id1");

        _sessionMock.Verify(s => s.Delete(It.IsAny<ProductDocument>()), Times.Never);
        _sessionMock.Verify(s => s.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}

// Helper for mocking IQueryable with RavenDB
public static class QueryableMockExtensions
{
    public static IRavenQueryable<T> BuildMock<T>(this IQueryable<T> data)
    {
        var mock = new Mock<IRavenQueryable<T>>();
        mock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
        mock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
        mock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
        return mock.Object;
    }
}