using Customer.Core.Interfaces;
using MediatR;

namespace Customer.Core.Features.Addresses.Commands.DeleteAddress
{
    public class DeleteAddressCommandHandler : IRequestHandler<DeleteAddressCommand, bool>
    {
        private readonly ICustomerRepository _repository;

        public DeleteAddressCommandHandler(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
        {
            return await _repository.DeleteAddressAsync(request.CustomerId, request.AddressId);
        }
    }
}
