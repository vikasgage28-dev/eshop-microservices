using EShop.Core.Features.Products.Queries;
using EShop.Core.Interfaces;
using EShop.Shared.DTOs;
using MediatR;

namespace EShop.Core.Features.Products.Handlers
{
    public class GetAllProductsHandler
        : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDto>>
    {
        private readonly IProductRepository _repository;

        public GetAllProductsHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductDto>> Handle(
            GetAllProductsQuery request,
            CancellationToken cancellationToken)
        {
            var products = await _repository.GetAllAsync();

            return products.Select(p => new ProductDto
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
        }
    }
}