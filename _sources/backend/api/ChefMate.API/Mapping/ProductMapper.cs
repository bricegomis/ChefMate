using ChefMate.API.Attributes;
using ChefMate.API.Mapping.Interfaces;
using ChefMate.API.Models.Documents;
using ChefMate.API.Models.Dto.Product;
using Riok.Mapperly.Abstractions;

namespace ChefMate.API.Mapping;

[Injectable(ServiceLifetime.Singleton)]
[Mapper]
public partial class ProductMapper : IProductMapper
{
    [MapperIgnoreSource(nameof(ProductDocument.ProfileId))]
    public partial ProductDto ToDto(ProductDocument doc);
    public partial List<ProductDto> ToDtoList(List<ProductDocument> docs);

    public partial PriceHistoryDto ToDto(PriceHistory priceHistory);
    public partial List<PriceHistoryDto> ToDtoList(List<PriceHistory> priceHistories);

    // Mapping for create/update DTOs  
    [MapperIgnoreTarget(nameof(ProductDocument.Id))]
    [MapperIgnoreTarget(nameof(ProductDocument.DateCreated))]
    [MapperIgnoreTarget(nameof(ProductDocument.DateModified))]
    public partial ProductDocument ToDocument(ProductCreateDto dto, string profileId);
    [MapperIgnoreTarget(nameof(ProductDocument.DateCreated))]
    [MapperIgnoreTarget(nameof(ProductDocument.DateModified))]
    public partial void UpdateDocument(ProductUpdateDto dto, ProductDocument document, string profileId);
}