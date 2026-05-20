using Customer.Core.Events;
using Customer.Core.Interfaces;
using MediatR;

namespace Customer.Core.Features.Customers.Commands.DeleteCustomer
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, bool>
    {
        private readonly ICustomerRepository _repository;
        private readonly IEventPublisher _eventPublisher;

        public DeleteCustomerCommandHandler(ICustomerRepository repository, IEventPublisher eventPublisher)
        {
            _repository = repository;
            _eventPublisher = eventPublisher;
        }

        public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repository.GetByIdAsync(request.Id);
            if (existing is null) return false;

            var deleted = await _repository.DeleteAsync(request.Id);

            if (deleted)
            {
                await _eventPublisher.PublishAsync(new CustomerDeletedEvent
                {
                    CustomerId = existing.Id,
                    Email      = existing.Email,
                    DeletedAt  = DateTime.UtcNow
                });
            }

            return deleted;
        }
    }
}
