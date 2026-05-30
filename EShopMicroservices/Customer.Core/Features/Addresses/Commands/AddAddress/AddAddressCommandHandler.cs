using Customer.Core.Entities;
using Customer.Core.Interfaces;
using MediatR;

namespace Customer.Core.Features.Addresses.Commands.AddAddress
{
    public class AddAddressCommandHandler : IRequestHandler<AddAddressCommand, Address?>
    {
        private readonly ICustomerRepository _repository;

        public AddAddressCommandHandler(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<Address?> Handle(AddAddressCommand request, CancellationToken cancellationToken)
        {
            var address = new Address
            {
                Street     = request.Street,
                City       = request.City,
                State      = request.State,
                Country    = request.Country,
                PostalCode = request.PostalCode,
                IsDefault  = request.IsDefault
            };

            return await _repository.AddAddressAsync(request.CustomerId, address);
        }
    }
}
