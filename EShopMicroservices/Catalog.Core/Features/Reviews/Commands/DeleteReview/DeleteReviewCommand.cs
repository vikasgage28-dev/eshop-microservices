using MediatR;

namespace Catalog.Core.Features.Reviews.Commands.DeleteReview
{
    public record DeleteReviewCommand(string Id, Guid ProductId) : IRequest<bool>;
}
