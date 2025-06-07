using ChefMate.API.Models.Documents;
using Raven.Client.Documents.Indexes;

namespace ChefMate.API.Indexes;

public class ProductTagsIndex : AbstractIndexCreationTask<ProductDocument, ProductTagsIndex.Result>
{
    public class Result
    {
        public string ProfileId { get; set; } = default!;
        public string Tag { get; set; } = default!;
    }

    public ProductTagsIndex()
    {
        Map = products => from product in products
                          from tag in product.Tags ?? new List<string>()
                          select new
                          {
                              product.ProfileId,
                              Tag = tag
                          };
    }
}