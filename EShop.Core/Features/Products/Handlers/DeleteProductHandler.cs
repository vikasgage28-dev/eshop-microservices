using EShop.Core.Features.Products.Commands;
using EShop.Core.Interfaces;
using MediatR;

namespace EShop.Core.Features.Products.Handlers
{
    public class DeleteProductHandler
        : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IProductRepository _repository;

        public DeleteProductHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeleteProductCommand request,
            CancellationToken cancellationToken)
        {
            // Just delegate to repository
            // Handler keeps it simple!
            return await _repository.DeleteAsync(request.Id);
        }
    }
}