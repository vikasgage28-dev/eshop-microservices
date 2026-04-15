using EShop.Core.Features.Products.Queries;
using EShop.Core.Interfaces;
using EShop.Shared.Common;
using EShop.Shared.DTOs;
using MediatR;

namespace EShop.Core.Features.Products.Handlers
{
    public class GetProductsHandler
        : IRequestHandler<GetProductsQuery, PagedResult<ProductDto>>
    {
        private readonly IProductRepository _repository;

        public GetProductsHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<ProductDto>> Handle(
            GetProductsQuery request,
            CancellationToken cancellationToken)
        {
            var (products, totalCount) = await _repository.GetPagedAsync(
                request.ValidatedPage,
                request.ValidatedPageSize,
                request.Search,
                request.Category);

            var dtos = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                Category = p.Category,
                CreatedAt = p.CreatedAt,
                IsActive = p.IsActive
            });

            return PagedResult<ProductDto>.Create(
                dtos,
                totalCount,
                request.ValidatedPage,
                request.ValidatedPageSize);
        }
    }
}
