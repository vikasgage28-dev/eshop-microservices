using EShop.Core.Entities;
using EShop.Core.Interfaces;
using EShop.Shared.Common;
using EShop.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EShop.API.Controllers
{
    [ApiController]
    [Route("api/v1/products/{productId}/reviews")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewsController(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<IEnumerable<ReviewDto>>>> GetByProduct(int productId)
        {
            var reviews = await _reviewRepository.GetByProductIdAsync(productId);
            var dtos = reviews.Select(r => new ReviewDto
            {
                Id = r.Id,
                ProductId = r.ProductId,
                UserEmail = r.UserEmail,
                Rating = r.Rating,
                Comment = r.Comment,
                VerifiedPurchase = r.VerifiedPurchase,
                CreatedAt = r.CreatedAt
            });
            return Ok(ApiResponse<IEnumerable<ReviewDto>>.Ok(dtos));
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ApiResponse<ReviewDto>>> Create(
            int productId, [FromBody] CreateReviewDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var userEmail = User.FindFirstValue(ClaimTypes.Email)!;

            var review = new Review
            {
                ProductId = productId,
                UserId = userId,
                UserEmail = userEmail,
                Rating = dto.Rating,
                Comment = dto.Comment,
                VerifiedPurchase = dto.VerifiedPurchase
            };

            var created = await _reviewRepository.CreateAsync(review);
            return Ok(ApiResponse<ReviewDto>.Ok(new ReviewDto
            {
                Id = created.Id,
                ProductId = created.ProductId,
                UserEmail = created.UserEmail,
                Rating = created.Rating,
                Comment = created.Comment,
                VerifiedPurchase = created.VerifiedPurchase,
                CreatedAt = created.CreatedAt
            }, "Review created successfully!"));
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int productId, string id)
        {
            await _reviewRepository.DeleteAsync(id, productId);
            return Ok(ApiResponse<bool>.Ok(true, "Review deleted successfully!"));
        }
    }
}