using Catalog.API.DTOs;
using Catalog.Core.Entities;
using Catalog.Core.Features.Products.Commands.CreateProduct;
using Catalog.Core.Features.Products.Commands.DeleteProduct;
using Catalog.Core.Features.Products.Commands.UpdateProduct;
using Catalog.Core.Features.Products.Queries.GetAllProducts;
using Catalog.Core.Features.Products.Queries.GetProductById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET api/products?page=1&pageSize=10&search=laptop&categoryId=...
        [HttpGet]
        public async Task<ActionResult<PagedResult<ProductDto>>> GetAll(
            [FromQuery] int     page       = 1,
            [FromQuery] int     pageSize   = 10,
            [FromQuery] string? search     = null,
            [FromQuery] Guid?   categoryId = null)
        {
            var (products, totalCount) = await _mediator.Send(
                new GetAllProductsQuery(page, pageSize, search, categoryId));

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return Ok(new PagedResult<ProductDto>(
                products.Select(ToDto), totalCount, page, pageSize, totalPages));
        }

        // GET api/products/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProductDto>> GetById(Guid id)
        {
            var product = await _mediator.Send(new GetProductByIdQuery(id));
            if (product is null) return NotFound();
            return Ok(ToDto(product));
        }

        // POST api/products
        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductRequest request)
        {
            var product = await _mediator.Send(new CreateProductCommand(
                request.Name, request.Description, request.Price, request.Stock, request.CategoryId));

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, ToDto(product));
        }

        // PUT api/products/{id}
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ProductDto>> Update(Guid id, [FromBody] UpdateProductRequest request)
        {
            var product = await _mediator.Send(new UpdateProductCommand(
                id, request.Name, request.Description, request.Price,
                request.Stock, request.CategoryId, request.IsActive));

            if (product is null) return NotFound();
            return Ok(ToDto(product));
        }

        // DELETE api/products/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _mediator.Send(new DeleteProductCommand(id));
            if (!deleted) return NotFound();
            return NoContent();
        }

        private static ProductDto ToDto(Product p) => new(
            p.Id, p.Name, p.Description, p.Price, p.Stock,
            p.IsActive, p.CreatedAt, p.CategoryId, p.Category?.Name ?? string.Empty);
    }
}
