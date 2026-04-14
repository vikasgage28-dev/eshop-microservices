using EShop.Core.Entities;
using EShop.Core.Features.Products.Commands;
using EShop.Core.Interfaces;
using EShop.Shared.DTOs;
using MediatR;

namespace EShop.Core.Features.Products.Handlers
{
    public class UpdateProductHandler
        : IRequestHandler<UpdateProductCommand, ProductDto?>
    {
        private readonly IProductRepository _repository;

        public UpdateProductHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductDto?> Handle(
            UpdateProductCommand request,
            CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Stock = request.Stock,
                Category = request.Category,
                IsActive = request.IsActive
            };

            var updated = await _repository.UpdateAsync(request.Id, product);
            if (updated == null) return null;

            return new ProductDto
            {
                Id = updated.Id,
                Name = updated.Name,
                Description = updated.Description,
                Price = updated.Price,
                Stock = updated.Stock,
                Category = updated.Category,
                CreatedAt = updated.CreatedAt,
                IsActive = updated.IsActive
            };
        }
    }
}