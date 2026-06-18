using Customer.Core.Entities;
using MediatR;

namespace Customer.Core.Features.Addresses.Commands.AddAddress
{
    public class AddAddressCommand : IRequest<Address?>
    {
        public Guid CustomerId { get; set; }
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public bool IsDefault { get; set; }
    }
}
