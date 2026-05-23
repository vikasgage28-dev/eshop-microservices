using MediatR;

namespace Customer.Core.Features.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommand : IRequest<Entities.Customer>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
    }
}
