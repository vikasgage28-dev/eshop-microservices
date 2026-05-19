using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using MediatR;

namespace Catalog.Core.Features.Reviews.Commands.CreateReview
{
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, Review>
    {
        private readonly IReviewRepository _repository;

        public CreateReviewCommandHandler(IReviewRepository repository)
        {
            _repository = repository;
        }

        public async Task<Review> Handle(
            CreateReviewCommand request,
            CancellationToken cancellationToken)
        {
            var review = new Review
            {
                ProductId        = request.ProductId,
                UserId           = request.UserId,
                UserEmail        = request.UserEmail,
                Rating           = request.Rating,
                Comment          = request.Comment,
                VerifiedPurchase = request.VerifiedPurchase
            };

            return await _repository.CreateAsync(review);
        }
    }
}
