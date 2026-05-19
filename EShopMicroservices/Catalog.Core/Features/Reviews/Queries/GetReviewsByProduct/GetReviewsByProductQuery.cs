using Catalog.Core.Entities;
using MediatR;

namespace Catalog.Core.Features.Reviews.Queries.GetReviewsByProduct
{
    public record GetReviewsByProductQuery(Guid ProductId) : IRequest<IEnumerable<Review>>;
}
