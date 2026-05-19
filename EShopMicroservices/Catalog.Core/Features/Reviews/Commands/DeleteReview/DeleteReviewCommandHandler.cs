using Catalog.Core.Interfaces;
using MediatR;

namespace Catalog.Core.Features.Reviews.Commands.DeleteReview
{
    public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand, bool>
    {
        private readonly IReviewRepository _repository;

        public DeleteReviewCommandHandler(IReviewRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeleteReviewCommand request,
            CancellationToken cancellationToken)
        {
            await _repository.DeleteAsync(request.Id, request.ProductId);
            return true;
        }
    }
}
