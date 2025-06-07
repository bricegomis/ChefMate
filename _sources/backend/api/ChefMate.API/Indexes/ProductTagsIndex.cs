using ChefMate.API.Models.Documents;
using Raven.Client.Documents.Indexes;

namespace ChefMate.API.Indexes;

public class ProductTagsIndex : AbstractIndexCreationTask<ProductDocument, ProductTagsIndex.Result>
{
    public class Result
    {
        public string ProfileId { get; set; } = default!;
        public string Tag { get; set; } = default!;
        public int Count { get; set; }
    }

    public ProductTagsIndex()
    {
        Map = products => from product in products
                          where product.ProfileId != null && product.Tags != null
                          from tag in product.Tags
                          select new
                          {
                              product.ProfileId,
                              Tag = tag,
                              Count = 1
                          };

        Reduce = results => from result in results
                            group result by new { result.ProfileId, result.Tag }
                            into g
                            select new
                            {
                                ProfileId = g.Key.ProfileId,
                                Tag = g.Key.Tag,
                                Count = g.Sum(x => x.Count)
                            };
    }
}