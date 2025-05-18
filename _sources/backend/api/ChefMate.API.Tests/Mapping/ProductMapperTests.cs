using System;
using System.Collections.Generic;
using ChefMate.API.Mapping;
using ChefMate.API.Models.Documents;
using ChefMate.API.Models.Dto;
using ChefMate.API.Models.Enums;
using FluentAssertions;
using Xunit;

namespace ChefMate.API.Tests.Mapping;

public class ProductMapperTests
{
    private readonly ProductMapper _mapper = new();

    [Fact]
    public void ToDto_Maps_All_Properties()
    {
        var doc = new ProductDocument
        {
            Id = "id1",
            ProfileId = "profile1",
            DateCreated = DateTimeOffset.UtcNow.AddDays(-1),
            DateModified = DateTimeOffset.UtcNow,
            Name = "Test",
            Description = "Desc",
            Image = "img.png",
            Labels = new List<string> { "A", "B" },
            Tags = new List<string> { "tag1" },
            Prices = new List<PriceHistory>
            {
                new PriceHistory
                {
                    StoreId = "store1",
                    Price = 1.5,
                    Quantity = 2,
                    Unit = ProductQuantityUnit.Kg,
                    DateBuying = DateTimeOffset.UtcNow.AddDays(-2)
                }
            },
            Usages = new List<ProductUsageType> { ProductUsageType.Food }
        };

        var dto = _mapper.ToDto(doc);

        dto.Id.Should().Be(doc.Id);
        dto.Name.Should().Be(doc.Name);
        dto.Description.Should().Be(doc.Description);
        dto.Image.Should().Be(doc.Image);
        dto.Labels.Should().BeEquivalentTo(doc.Labels);
        dto.Tags.Should().BeEquivalentTo(doc.Tags);
        dto.Usages.Should().BeEquivalentTo(doc.Usages);
        dto.DateCreated.Should().Be(doc.DateCreated);
        dto.DateModified.Should().Be(doc.DateModified);
        dto.Prices.Should().NotBeNull();
        dto.Prices.Should().HaveCount(1);
        dto.Prices![0].StoreId.Should().Be("store1");
    }

    [Fact]
    public void ToDtoList_Maps_List()
    {
        var docs = new List<ProductDocument>
        {
            new ProductDocument
            {
                Id = "id1",
                ProfileId = "profile1",
                DateCreated = DateTimeOffset.UtcNow,
                DateModified = DateTimeOffset.UtcNow,
                Name = "Test1"
            },
            new ProductDocument
            {
                Id = "id2",
                ProfileId = "profile2",
                DateCreated = DateTimeOffset.UtcNow,
                DateModified = DateTimeOffset.UtcNow,
                Name = "Test2"
            }
        };

        var dtos = _mapper.ToDtoList(docs);

        dtos.Should().HaveCount(2);
        dtos[0].Id.Should().Be("id1");
        dtos[1].Id.Should().Be("id2");
    }

    [Fact]
    public void ToDto_Maps_PriceHistory()
    {
        var price = new PriceHistory
        {
            StoreId = "storeX",
            Price = 9.99,
            Quantity = 3,
            Unit = ProductQuantityUnit.Piece,
            DateBuying = DateTimeOffset.UtcNow.AddDays(-5)
        };

        var dto = _mapper.ToDto(price);

        dto.StoreId.Should().Be(price.StoreId);
        dto.Price.Should().Be(price.Price);
        dto.Quantity.Should().Be(price.Quantity);
        dto.Unit.Should().Be(price.Unit);
        dto.DateBuying.Should().Be(price.DateBuying);
    }

    [Fact]
    public void ToDocument_Maps_CreateDto()
    {
        var dto = new ProductCreateDto
        {
            Name = "New",
            Description = "Desc",
            Image = "img.png",
            Labels = new List<string> { "A" },
            Tags = new List<string> { "tag" },
            Usages = new List<ProductUsageType> { ProductUsageType.Food }
        };
        var profileId = "profileX";

        var doc = _mapper.ToDocument(dto, profileId);

        doc.Name.Should().Be(dto.Name);
        doc.Description.Should().Be(dto.Description);
        doc.Image.Should().Be(dto.Image);
        doc.Labels.Should().BeEquivalentTo(dto.Labels);
        doc.Tags.Should().BeEquivalentTo(dto.Tags);
        doc.Usages.Should().BeEquivalentTo(dto.Usages);
        doc.ProfileId.Should().BeEquivalentTo(profileId);
    }

    [Fact]
    public void UpdateDocument_Maps_UpdateDto()
    {
        var profileId = "profile1";
        var doc = new ProductDocument
        {
            Id = "id1",
            ProfileId = profileId,
            DateCreated = DateTimeOffset.UtcNow.AddDays(-10),
            DateModified = DateTimeOffset.UtcNow.AddDays(-5),
            Name = "Old",
            Description = "OldDesc",
            Image = "old.png",
            Labels = new List<string> { "X" },
            Tags = new List<string> { "old" },
            Usages = new List<ProductUsageType> { ProductUsageType.Other }
        };

        var dto = new ProductUpdateDto
        {
            Id = "id1",
            Name = "Updated",
            Description = "NewDesc",
            Image = "new.png",
            Labels = new List<string> { "Y" },
            Tags = new List<string> { "new" },
            Usages = new List<ProductUsageType> { ProductUsageType.Food }
        };

        _mapper.UpdateDocument(dto, doc, profileId);

        doc.Name.Should().Be(dto.Name);
        doc.Description.Should().Be(dto.Description);
        doc.Image.Should().Be(dto.Image);
        doc.Labels.Should().BeEquivalentTo(dto.Labels);
        doc.Tags.Should().BeEquivalentTo(dto.Tags);
        doc.Usages.Should().BeEquivalentTo(dto.Usages);
        doc.Id.Should().Be(dto.Id);
        doc.ProfileId.Should().Be(profileId);
    }
}