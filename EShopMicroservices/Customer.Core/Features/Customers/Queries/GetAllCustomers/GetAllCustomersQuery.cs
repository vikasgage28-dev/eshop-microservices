using MediatR;

namespace Customer.Core.Features.Customers.Queries.GetAllCustomers
{
    public class GetAllCustomersQuery : IRequest<IEnumerable<Entities.Customer>> { }
}
