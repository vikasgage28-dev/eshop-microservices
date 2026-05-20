using Customer.Core.Interfaces;
using MediatR;

namespace Customer.Core.Features.Customers.Queries.GetCustomerById
{
    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, Entities.Customer?>
    {
        private readonly ICustomerRepository _repository;

        public GetCustomerByIdQueryHandler(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<Entities.Customer?> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
            => await _repository.GetByIdAsync(request.Id);
    }
}
