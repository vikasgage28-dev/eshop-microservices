using EShop.Core.Entities;
using EShop.Core.Features.Products.Commands;
using EShop.Core.Interfaces;
using EShop.Shared.DTOs;
using MediatR;

namespace EShop.Core.Features.Products.Handlers
{
    public class CreateProductHandler
        : IRequestHandler<CreateProductCommand, ProductDto>
    {
        private readonly IProductRepository _repository;

        public CreateProductHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductDto> Handle(
            CreateProductCommand request,
            CancellationToken cancellationToken)
        {
            // Map Command → Entity
            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Stock = request.Stock,
                Category = request.Category
            };

            var created = await _repository.CreateAsync(product);

            // Map Entity → DTO and return
            return new ProductDto
            {
                Id = created.Id,
                Name = created.Name,
                Description = created.Description,
                Price = created.Price,
                Stock = created.Stock,
                Category = created.Category,
                CreatedAt = created.CreatedAt,
                IsActive = created.IsActive
            };
        }
    }
}