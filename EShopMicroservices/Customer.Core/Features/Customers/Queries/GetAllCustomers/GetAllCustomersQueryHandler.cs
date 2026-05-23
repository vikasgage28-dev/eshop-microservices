using Customer.Core.Interfaces;
using MediatR;

namespace Customer.Core.Features.Customers.Queries.GetAllCustomers
{
    public class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, IEnumerable<Entities.Customer>>
    {
        private readonly ICustomerRepository _repository;

        public GetAllCustomersQueryHandler(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Entities.Customer>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
            => await _repository.GetAllAsync();
    }
}
