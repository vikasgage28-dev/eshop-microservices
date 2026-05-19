using Catalog.Core.Entities;
using Catalog.Core.Events;
using Catalog.Core.Interfaces;
using MediatR;

namespace Catalog.Core.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Product>
    {
        private readonly IProductRepository _repository;
        private readonly IEventPublisher    _eventPublisher;

        public CreateProductCommandHandler(
            IProductRepository repository,
            IEventPublisher    eventPublisher)
        {
            _repository     = repository;
            _eventPublisher = eventPublisher;
        }

        public async Task<Product> Handle(
            CreateProductCommand request,
            CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name        = request.Name,
                Description = request.Description,
                Price       = request.Price,
                Stock       = request.Stock,
                CategoryId  = request.CategoryId
            };

            // Step 1: Save to database
            var created = await _repository.CreateAsync(product);

            // Step 2: Publish event AFTER successful save!
            await _eventPublisher.PublishAsync(new ProductCreatedEvent(
                created.Id,
                created.Name,
                created.Price,
                created.Stock,
                created.CategoryId,
                DateTime.UtcNow), cancellationToken);

            return created;
        }
    }
}
