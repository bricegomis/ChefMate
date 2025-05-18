using ChefMate.API.Mapping.Interfaces;
using ChefMate.API.Models.Documents;
using ChefMate.API.Models.Dto;
using ChefMate.API.Repositories;
using ChefMate.API.Services;
using ChefMate.API.Services.Interfaces;
using FluentAssertions;
using Moq;

namespace ChefMate.API.Tests.Services;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _repoMock = new();
    private readonly Mock<IProductMapper> _mapperMock = new();
    private readonly IProductService _service;

    public ProductServiceTests()
    {
        _service = new ProductService(_repoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_Returns_Products()
    {
        var profileId = "profile1";
        var products = new List<ProductDocument> { new() { Id = "id1", ProfileId = profileId, Name = "P" } };
        _repoMock.Setup(r => r.GetAllAsync(profileId)).ReturnsAsync(products);

        var result = await _service.GetAllAsync(profileId);

        result.Should().BeEquivalentTo(products);
    }

    [Fact]
    public async Task GetByIdAsync_Returns_Product()
    {
        var doc = new ProductDocument { Id = "id1", ProfileId = "profile1", Name = "P" };
        _repoMock.Setup(r => r.GetByIdAsync("id1")).ReturnsAsync(doc);

        var result = await _service.GetByIdAsync("id1");

        result.Should().Be(doc);
    }

    [Fact]
    public async Task AddAsync_Maps_And_Adds_Product()
    {
        var dto = new ProductCreateDto { Name = "N" };
        var profileId = "profileX";
        var doc = new ProductDocument { Name = "N", ProfileId = profileId };
        _mapperMock.Setup(m => m.ToDocument(dto, profileId)).Returns(doc);

        var result = await _service.AddAsync(dto, profileId);

        _repoMock.Verify(r => r.AddAsync(doc), Times.Once);
        result.Should().Be(doc);
    }

    [Fact]
    public async Task UpdateAsync_Updates_Product_When_ProfileId_Matches()
    {
        var profileId = "profile1";
        var doc = new ProductDocument { Id = "id1", ProfileId = profileId, Name = "Old" };
        var dto = new ProductUpdateDto { Id = "id1", Name = "New" };
        _repoMock.Setup(r => r.GetByIdAsync("id1")).ReturnsAsync(doc);

        var result = await _service.UpdateAsync(dto, profileId);

        _mapperMock.Verify(m => m.UpdateDocument(dto, doc, profileId), Times.Once);
        _repoMock.Verify(r => r.UpdateAsync(doc), Times.Once);
        result.Should().Be(doc);
    }

    [Fact]
    public async Task UpdateAsync_Throws_When_ProfileId_DoesNotMatch()
    {
        var doc = new ProductDocument { Id = "id1", ProfileId = "profileX", Name = "Old" };
        var dto = new ProductUpdateDto { Id = "id1", Name = "New" };
        _repoMock.Setup(r => r.GetByIdAsync("id1")).ReturnsAsync(doc);

        Func<Task> act = async () => await _service.UpdateAsync(dto, "profileY");

        await act.Should().ThrowAsync<ApplicationException>().WithMessage("Profile mismatch.");
    }

    [Fact]
    public async Task DeleteAsync_Deletes_When_ProfileId_Matches()
    {
        var doc = new ProductDocument { Id = "id1", ProfileId = "profile1", Name = "name1" };
        _repoMock.Setup(r => r.GetByIdAsync("id1")).ReturnsAsync(doc);

        var result = await _service.DeleteAsync("id1", "profile1");

        _repoMock.Verify(r => r.DeleteAsync("id1"), Times.Once);
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_Throws_When_ProfileId_DoesNotMatch()
    {
        var doc = new ProductDocument { Id = "id1", ProfileId = "profileX", Name = "name1" };
        _repoMock.Setup(r => r.GetByIdAsync("id1")).ReturnsAsync(doc);

        Func<Task> act = async () => await _service.DeleteAsync("id1", "profileY");

        await act.Should().ThrowAsync<ApplicationException>().WithMessage("Profile mismatch.");
    }
}