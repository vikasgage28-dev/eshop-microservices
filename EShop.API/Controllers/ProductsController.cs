using Asp.Versioning;
using EShop.Core.Features.Products.Commands;
using EShop.Core.Features.Products.Queries;
using EShop.Shared.Common;
using EShop.Shared.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShop.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ─────────────────────────────────────────────
        // V1 ONLY - Returns flat list (old clients)
        // ─────────────────────────────────────────────
        [HttpGet]
        [AllowAnonymous]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProductDto>>>> GetAllV1()
        {
            var result = await _mediator.Send(new GetAllProductsQuery());
            return Ok(ApiResponse<IEnumerable<ProductDto>>.Ok(result));
        }

        // ─────────────────────────────────────────────
        // V2 ONLY - Returns PagedResult by default
        // Breaking change from V1!
        // ─────────────────────────────────────────────
        [HttpGet]
        [AllowAnonymous]
        [MapToApiVersion("2.0")]
        public async Task<ActionResult<ApiResponse<PagedResult<ProductDto>>>> GetAllV2(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? category = null)
        {
            var query = new GetProductsQuery
            {
                Page = page,
                PageSize = pageSize,
                Search = search,
                Category = category
            };
            var result = await _mediator.Send(query);
            return Ok(ApiResponse<PagedResult<ProductDto>>.Ok(result));
        }

        // ─────────────────────────────────────────────
        // SHARED - Works same in V1 and V2
        // ─────────────────────────────────────────────

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<ProductDto>>> GetById(int id)
        {
            var result = await _mediator.Send(new GetProductByIdQuery(id));
            if (result == null)
                return NotFound(ApiResponse<ProductDto>.Fail($"Product {id} not found"));

            return Ok(ApiResponse<ProductDto>.Ok(result));
        }

        [HttpGet("category/{category}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProductDto>>>> GetByCategory(
            string category)
        {
            var result = await _mediator.Send(new GetProductsByCategoryQuery(category));
            return Ok(ApiResponse<IEnumerable<ProductDto>>.Ok(result));
        }

        [HttpGet("paged")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<PagedResult<ProductDto>>>> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? category = null)
        {
            var query = new GetProductsQuery
            {
                Page = page,
                PageSize = pageSize,
                Search = search,
                Category = category
            };
            var result = await _mediator.Send(query);
            return Ok(ApiResponse<PagedResult<ProductDto>>.Ok(result));
        }

        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<ApiResponse<ProductDto>>> Create(
            [FromBody] CreateProductCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id },
                ApiResponse<ProductDto>.Ok(result, "Product created successfully"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<ApiResponse<ProductDto>>> Update(
            int id, [FromBody] UpdateProductCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            if (result == null)
                return NotFound(ApiResponse<ProductDto>.Fail($"Product {id} not found"));

            return Ok(ApiResponse<ProductDto>.Ok(result, "Product updated successfully"));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteProductCommand(id));
            if (!result)
                return NotFound(ApiResponse<bool>.Fail($"Product {id} not found"));

            return Ok(ApiResponse<bool>.Ok(true, "Product deleted successfully"));
        }
    }
}