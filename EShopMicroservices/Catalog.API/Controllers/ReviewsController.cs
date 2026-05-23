using Catalog.API.DTOs;
using Catalog.Core.Entities;
using Catalog.Core.Features.Reviews.Commands.CreateReview;
using Catalog.Core.Features.Reviews.Commands.DeleteReview;
using Catalog.Core.Features.Reviews.Queries.GetReviewsByProduct;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReviewsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET api/reviews?productId={guid}
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetByProduct([FromQuery] Guid productId)
        {
            var reviews = await _mediator.Send(new GetReviewsByProductQuery(productId));
            return Ok(reviews.Select(ToDto));
        }

        // POST api/reviews
        [HttpPost]
        public async Task<ActionResult<ReviewDto>> Create([FromBody] CreateReviewRequest request)
        {
            var review = await _mediator.Send(new CreateReviewCommand(
                request.ProductId, request.UserId, request.UserEmail,
                request.Rating, request.Comment, request.VerifiedPurchase));

            return StatusCode(201, ToDto(review));
        }

        // DELETE api/reviews/{id}?productId={guid}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, [FromQuery] Guid productId)
        {
            await _mediator.Send(new DeleteReviewCommand(id, productId));
            return NoContent();
        }

        private static ReviewDto ToDto(Review r) => new(
            r.Id, r.ProductId, r.UserId, r.UserEmail,
            r.Rating, r.Comment, r.VerifiedPurchase, r.CreatedAt);
    }
}
