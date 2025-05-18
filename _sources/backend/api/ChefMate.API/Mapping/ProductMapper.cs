using Riok.Mapperly.Abstractions;
using ChefMate.API.Models.Documents;
using ChefMate.API.Models.Dto;

namespace ChefMate.API.Mapping;

[Mapper]
public partial class ProductMapper
{
    public partial ProductDto ToDto(ProductDocument doc);
    public partial List<ProductDto> ToDtoList(List<ProductDocument> docs);
}