using MediatR;

namespace Customer.Core.Features.Customers.Queries.GetCustomerByEmail
{
    public class GetCustomerByEmailQuery : IRequest<Entities.Customer?>
    {
        public string Email { get; }
        public GetCustomerByEmailQuery(string email) => Email = email;
    }
}
