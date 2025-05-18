using ChefMate.API.Models.Documents;
using ChefMate.API.Models.Dto;
using Riok.Mapperly.Abstractions;

namespace ChefMate.API.Mapping;

[Mapper]
public partial class ProductMapper
{
    [MapperIgnoreSource(nameof(ProductDocument.ProfileId))]
    public partial ProductDto ToDto(ProductDocument doc);
    public partial List<ProductDto> ToDtoList(List<ProductDocument> docs);

    public partial PriceHistoryDto ToDto(PriceHistory priceHistory);
    public partial List<PriceHistoryDto> ToDtoList(List<PriceHistory> priceHistories);
}