using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using MediatR;

namespace Catalog.Core.Features.Reviews.Queries.GetReviewsByProduct
{
    public class GetReviewsByProductQueryHandler
        : IRequestHandler<GetReviewsByProductQuery, IEnumerable<Review>>
    {
        private readonly IReviewRepository _repository;

        public GetReviewsByProductQueryHandler(IReviewRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Review>> Handle(
            GetReviewsByProductQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetByProductIdAsync(request.ProductId);
        }
    }
}
