using ChefMate.API.Mapping.Interfaces;
using ChefMate.API.Models.Documents;
using ChefMate.API.Models.Dto.Store;
using ChefMate.API.Repositories.Interfaces;
using ChefMate.API.Services;
using FluentAssertions;
using Moq;

namespace ChefMate.API.Tests.Services;

public class StoreServiceTests
{
    private readonly Mock<IStoreRepository> _repoMock = new();
    private readonly Mock<IStoreMapper> _mapperMock = new();

    private StoreService CreateService()
        => new(_repoMock.Object, _mapperMock.Object);

    [Fact]
    public async Task AddAsync_Calls_Mapper_And_Repo()
    {
        var dto = new StoreCreateDto { Name = "S" };
        var doc = new StoreDocument { Name = "S", ProfileId = "p1" };
        _mapperMock.Setup(m => m.ToDocument(dto, "p1")).Returns(doc);

        var service = CreateService();
        var result = await service.AddAsync(dto, "p1");

        result.Should().Be(doc);
        _repoMock.Verify(r => r.AddAsync(doc), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_Throws_On_Profile_Mismatch()
    {
        var dto = new StoreUpdateDto { Id = "id1", Name = "S" };
        var doc = new StoreDocument { Id = "id1", Name = "S", ProfileId = "other" };
        _repoMock.Setup(r => r.GetByIdAsync("id1")).ReturnsAsync(doc);

        var service = CreateService();
        await Assert.ThrowsAsync<ApplicationException>(() => service.UpdateAsync(dto, "p1"));
    }

    [Fact]
    public async Task DeleteAsync_ByProfile_Throws_On_Profile_Mismatch()
    {
        var doc = new StoreDocument { Id = "id1", Name = "S", ProfileId = "other" };
        _repoMock.Setup(r => r.GetByIdAsync("id1")).ReturnsAsync(doc);

        var service = CreateService();
        await Assert.ThrowsAsync<ApplicationException>(() => service.DeleteAsync("id1", "p1"));
    }
}