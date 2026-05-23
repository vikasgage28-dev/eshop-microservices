using Catalog.Core.Entities;
using Catalog.Core.Events;
using Catalog.Core.Interfaces;
using MediatR;

namespace Catalog.Core.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Product?>
    {
        private readonly IProductRepository _repository;
        private readonly IEventPublisher    _eventPublisher;

        public UpdateProductCommandHandler(
            IProductRepository repository,
            IEventPublisher    eventPublisher)
        {
            _repository     = repository;
            _eventPublisher = eventPublisher;
        }

        public async Task<Product?> Handle(
            UpdateProductCommand request,
            CancellationToken cancellationToken)
        {
            // Step 1: Save to database
            var updated = await _repository.UpdateAsync(request.Id, new Product
            {
                Name        = request.Name,
                Description = request.Description,
                Price       = request.Price,
                Stock       = request.Stock,
                CategoryId  = request.CategoryId,
                IsActive    = request.IsActive
            });

            if (updated is null) return null;

            // Step 2: Publish event AFTER successful update!
            await _eventPublisher.PublishAsync(new ProductUpdatedEvent(
                updated.Id,
                updated.Name,
                updated.Price,
                updated.Stock,
                updated.CategoryId,
                updated.IsActive,
                DateTime.UtcNow), cancellationToken);

            return updated;
        }
    }
}
