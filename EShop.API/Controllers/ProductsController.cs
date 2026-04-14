using EShop.Core.Entities;
using EShop.Core.Interfaces;
using EShop.Shared.Common;
using EShop.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EShop.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProductDto>>>> GetAll()
        {
            var products = await _productRepository.GetAllAsync();
            var dtos = products.Select(MapToDto);
            return Ok(ApiResponse<IEnumerable<ProductDto>>.Ok(dtos, "Products retrieved successfully"));
        }

        // GET: api/products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ProductDto>>> GetById(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                return NotFound(ApiResponse<ProductDto>.Fail($"Product with ID {id} not found"));

            return Ok(ApiResponse<ProductDto>.Ok(MapToDto(product)));
        }

        // GET: api/products/category/Electronics
        [HttpGet("category/{category}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProductDto>>>> GetByCategory(string category)
        {
            var products = await _productRepository.GetByCategoryAsync(category);
            var dtos = products.Select(MapToDto);
            return Ok(ApiResponse<IEnumerable<ProductDto>>.Ok(dtos, $"Products in category '{category}'"));
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<ApiResponse<ProductDto>>> Create([FromBody] CreateProductDto createDto)
        {
            var product = new Product
            {
                Name = createDto.Name,
                Description = createDto.Description,
                Price = createDto.Price,
                Stock = createDto.Stock,
                Category = createDto.Category
            };

            var created = await _productRepository.CreateAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = created.Id },
                ApiResponse<ProductDto>.Ok(MapToDto(created), "Product created successfully"));
        }

        // PUT: api/products/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<ProductDto>>> Update(int id, [FromBody] UpdateProductDto updateDto)
        {
            var product = new Product
            {
                Name = updateDto.Name,
                Description = updateDto.Description,
                Price = updateDto.Price,
                Stock = updateDto.Stock,
                Category = updateDto.Category,
                IsActive = updateDto.IsActive
            };

            var updated = await _productRepository.UpdateAsync(id, product);
            if (updated == null)
                return NotFound(ApiResponse<ProductDto>.Fail($"Product with ID {id} not found"));

            return Ok(ApiResponse<ProductDto>.Ok(MapToDto(updated), "Product updated successfully"));
        }

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            var result = await _productRepository.DeleteAsync(id);
            if (!result)
                return NotFound(ApiResponse<bool>.Fail($"Product with ID {id} not found"));

            return Ok(ApiResponse<bool>.Ok(true, "Product deleted successfully"));
        }

        private static ProductDto MapToDto(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                Category = product.Category,
                CreatedAt = product.CreatedAt,
                IsActive = product.IsActive
            };
        }
    }
}
