using Catalog.Core.Events;
using Catalog.Core.Interfaces;
using MediatR;

namespace Catalog.Core.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IProductRepository _repository;
        private readonly IEventPublisher    _eventPublisher;

        public DeleteProductCommandHandler(
            IProductRepository repository,
            IEventPublisher    eventPublisher)
        {
            _repository     = repository;
            _eventPublisher = eventPublisher;
        }

        public async Task<bool> Handle(
            DeleteProductCommand request,
            CancellationToken cancellationToken)
        {
            // Step 1: Delete from database
            var deleted = await _repository.DeleteAsync(request.Id);

            if (!deleted) return false;

            // Step 2: Publish event AFTER successful delete!
            await _eventPublisher.PublishAsync(new ProductDeletedEvent(
                request.Id,
                DateTime.UtcNow), cancellationToken);

            return true;
        }
    }
}
