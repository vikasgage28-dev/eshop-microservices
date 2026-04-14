using EShop.Core.Features.Products.Queries;
using EShop.Core.Interfaces;
using EShop.Shared.DTOs;
using MediatR;

namespace EShop.Core.Features.Products.Handlers
{
    public class GetProductByIdHandler
        : IRequestHandler<GetProductByIdQuery, ProductDto?>
    {
        private readonly IProductRepository _repository;

        public GetProductByIdHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductDto?> Handle(
            GetProductByIdQuery request,
            CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.Id);
            if (product == null) return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                Category = product.Category,
                CreatedAt = product.CreatedAt,
                IsActive = product.IsActive
            };
        }
    }
}