using MediatR;

namespace Customer.Core.Features.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommand : IRequest<Entities.Customer?>
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
    }
}
