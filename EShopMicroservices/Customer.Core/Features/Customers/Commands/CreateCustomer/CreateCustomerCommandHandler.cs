using Customer.Core.Events;
using Customer.Core.Interfaces;
using MediatR;

namespace Customer.Core.Features.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Entities.Customer>
    {
        private readonly ICustomerRepository _repository;
        private readonly IEventPublisher _eventPublisher;

        public CreateCustomerCommandHandler(ICustomerRepository repository, IEventPublisher eventPublisher)
        {
            _repository = repository;
            _eventPublisher = eventPublisher;
        }

        public async Task<Entities.Customer> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = new Entities.Customer
            {
                FirstName = request.FirstName,
                LastName  = request.LastName,
                Email     = request.Email,
                Phone     = request.Phone,
                CreatedAt = DateTime.UtcNow
            };

            var saved = await _repository.AddAsync(customer);

            await _eventPublisher.PublishAsync(new CustomerCreatedEvent
            {
                CustomerId = saved.Id,
                FullName   = saved.FullName,
                Email      = saved.Email,
                CreatedAt  = saved.CreatedAt
            });

            return saved;
        }
    }
}
