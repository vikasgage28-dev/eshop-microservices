using Catalog.Core.Entities;
using MediatR;

namespace Catalog.Core.Features.Reviews.Commands.CreateReview
{
    public record CreateReviewCommand(
        Guid   ProductId,
        string UserId,
        string UserEmail,
        int    Rating,
        string Comment,
        bool   VerifiedPurchase
    ) : IRequest<Review>;
}
