using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using MediatR;

namespace Catalog.Core.Features.Products.Queries.GetAllProducts
{
    public class GetAllProductsQueryHandler
        : IRequestHandler<GetAllProductsQuery, (IEnumerable<Product> Products, int TotalCount)>
    {
        private readonly IProductRepository _repository;

        public GetAllProductsQueryHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<(IEnumerable<Product> Products, int TotalCount)> Handle(
            GetAllProductsQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetPagedAsync(
                request.Page,
                request.PageSize,
                request.Search,
                request.CategoryId);
        }
    }
}
