using ChefMate.API.Models.Documents;
using ChefMate.API.Models.Dto;

namespace ChefMate.API.Mapping.Interfaces;

public interface IProductMapper
{
    ProductDto ToDto(ProductDocument doc);
    List<ProductDto> ToDtoList(List<ProductDocument> docs);
    PriceHistoryDto ToDto(PriceHistory priceHistory);
    List<PriceHistoryDto> ToDtoList(List<PriceHistory> priceHistories);
    ProductDocument ToDocument(ProductCreateDto dto, string profileId);
    void UpdateDocument(ProductUpdateDto dto, ProductDocument document, string profileId);
}