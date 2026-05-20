using Customer.Core.Interfaces;
using MediatR;

namespace Customer.Core.Features.Customers.Queries.GetCustomerByEmail
{
    public class GetCustomerByEmailQueryHandler : IRequestHandler<GetCustomerByEmailQuery, Entities.Customer?>
    {
        private readonly ICustomerRepository _repository;

        public GetCustomerByEmailQueryHandler(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<Entities.Customer?> Handle(GetCustomerByEmailQuery request, CancellationToken cancellationToken)
            => await _repository.GetByEmailAsync(request.Email);
    }
}
