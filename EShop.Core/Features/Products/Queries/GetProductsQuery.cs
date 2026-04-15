using EShop.Shared.Common;
using EShop.Shared.DTOs;
using MediatR;

namespace EShop.Core.Features.Products.Queries
{
    public class GetProductsQuery : IRequest<PagedResult<ProductDto>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Search { get; set; }
        public string? Category { get; set; }

        public int ValidatedPage => Page < 1 ? 1 : Page;

        public int ValidatedPageSize => PageSize < 1 ? 10
                                      : PageSize > 100 ? 100
                                      : PageSize;
    }
}
