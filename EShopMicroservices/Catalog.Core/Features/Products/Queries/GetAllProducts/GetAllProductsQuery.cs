using Catalog.Core.Entities;
using MediatR;

namespace Catalog.Core.Features.Products.Queries.GetAllProducts
{
    public record GetAllProductsQuery(
        int     Page       = 1,
        int     PageSize   = 10,
        string? Search     = null,
        Guid?   CategoryId = null
    ) : IRequest<(IEnumerable<Product> Products, int TotalCount)>;
}
