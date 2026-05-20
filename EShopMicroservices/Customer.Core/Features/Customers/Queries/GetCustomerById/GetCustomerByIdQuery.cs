using MediatR;

namespace Customer.Core.Features.Customers.Queries.GetCustomerById
{
    public class GetCustomerByIdQuery : IRequest<Entities.Customer?>
    {
        public Guid Id { get; }
        public GetCustomerByIdQuery(Guid id) => Id = id;
    }
}
