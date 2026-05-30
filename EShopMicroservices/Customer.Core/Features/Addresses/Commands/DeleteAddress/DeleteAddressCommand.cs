using MediatR;

namespace Customer.Core.Features.Addresses.Commands.DeleteAddress
{
    public class DeleteAddressCommand : IRequest<bool>
    {
        public Guid CustomerId { get; set; }
        public Guid AddressId { get; set; }
    }
}
