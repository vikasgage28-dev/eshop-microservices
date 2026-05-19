using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using MediatR;

namespace Catalog.Core.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Product?>
    {
        private readonly IProductRepository _repository;

        public UpdateProductCommandHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<Product?> Handle(
            UpdateProductCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.UpdateAsync(request.Id, new Product
            {
                Name        = request.Name,
                Description = request.Description,
                Price       = request.Price,
                Stock       = request.Stock,
                CategoryId  = request.CategoryId,
                IsActive    = request.IsActive
            });
        }
    }
}
