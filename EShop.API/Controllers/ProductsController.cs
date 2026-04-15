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
    [Route("api/[controller]")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Products
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProductDto>>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllProductsQuery());
            return Ok(ApiResponse<IEnumerable<ProductDto>>.Ok(result));
        }

        // GET: api/Products/paged?page=1&pageSize=10&search=laptop&category=Electronics
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

        // GET: api/Products/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<ProductDto>>> GetById(int id)
        {
            var result = await _mediator.Send(new GetProductByIdQuery(id));
            if (result == null)
                return NotFound(ApiResponse<ProductDto>.Fail($"Product {id} not found"));

            return Ok(ApiResponse<ProductDto>.Ok(result));
        }

        // GET: api/Products/category/Electronics
        [HttpGet("category/{category}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProductDto>>>> GetByCategory(
            string category)
        {
            var result = await _mediator.Send(new GetProductsByCategoryQuery(category));
            return Ok(ApiResponse<IEnumerable<ProductDto>>.Ok(result));
        }

        // POST: api/Products
        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<ApiResponse<ProductDto>>> Create(
            [FromBody] CreateProductCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id },
                ApiResponse<ProductDto>.Ok(result, "Product created successfully"));
        }

        // PUT: api/Products/5
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

        // DELETE: api/Products/5
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