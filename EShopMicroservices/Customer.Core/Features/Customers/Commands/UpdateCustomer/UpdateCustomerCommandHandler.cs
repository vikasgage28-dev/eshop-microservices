using Customer.Core.Events;
using Customer.Core.Interfaces;
using MediatR;

namespace Customer.Core.Features.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Entities.Customer?>
    {
        private readonly ICustomerRepository _repository;
        private readonly IEventPublisher _eventPublisher;

        public UpdateCustomerCommandHandler(ICustomerRepository repository, IEventPublisher eventPublisher)
        {
            _repository = repository;
            _eventPublisher = eventPublisher;
        }

        public async Task<Entities.Customer?> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repository.GetByIdAsync(request.Id);
            if (existing is null) return null;

            existing.FirstName = request.FirstName;
            existing.LastName  = request.LastName;
            existing.Email     = request.Email;
            existing.Phone     = request.Phone;
            existing.UpdatedAt = DateTime.UtcNow;

            var updated = await _repository.UpdateAsync(existing);

            if (updated is not null)
            {
                await _eventPublisher.PublishAsync(new CustomerUpdatedEvent
                {
                    CustomerId = updated.Id,
                    FullName   = updated.FullName,
                    Email      = updated.Email,
                    UpdatedAt  = updated.UpdatedAt!.Value
                });
            }

            return updated;
        }
    }
}
