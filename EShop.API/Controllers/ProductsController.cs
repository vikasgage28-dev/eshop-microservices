using EShop.Core.Entities;
using EShop.Core.Interfaces;
using EShop.Shared.Common;
using EShop.Shared.DTOs;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShop.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IValidator<CreateProductDto> _createValidator;
        private readonly IValidator<UpdateProductDto> _updateValidator;

        public ProductsController(
            IProductRepository productRepository,
            IValidator<CreateProductDto> createValidator,
            IValidator<UpdateProductDto> updateValidator)
        {
            _productRepository = productRepository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProductDto>>>> GetAll()
        {
            var products = await _productRepository.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<ProductDto>>.Ok(products.Select(MapToDto), "Products retrieved successfully"));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<ProductDto>>> GetById(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                return NotFound(ApiResponse<ProductDto>.Fail($"Product with ID {id} not found"));

            return Ok(ApiResponse<ProductDto>.Ok(MapToDto(product)));
        }

        [HttpGet("category/{category}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProductDto>>>> GetByCategory(string category)
        {
            var products = await _productRepository.GetByCategoryAsync(category);
            return Ok(ApiResponse<IEnumerable<ProductDto>>.Ok(products.Select(MapToDto), $"Products in '{category}'"));
        }

        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<ApiResponse<ProductDto>>> Create([FromBody] CreateProductDto createDto)
        {
            var validation = await _createValidator.ValidateAsync(createDto);
            if (!validation.IsValid)
            {
                var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(ApiResponse<ProductDto>.Fail("Validation failed", errors));
            }

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

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<ApiResponse<ProductDto>>> Update(int id, [FromBody] UpdateProductDto updateDto)
        {
            var validation = await _updateValidator.ValidateAsync(updateDto);
            if (!validation.IsValid)
            {
                var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(ApiResponse<ProductDto>.Fail("Validation failed", errors));
            }

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

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            var result = await _productRepository.DeleteAsync(id);
            if (!result)
                return NotFound(ApiResponse<bool>.Fail($"Product with ID {id} not found"));

            return Ok(ApiResponse<bool>.Ok(true, "Product deleted successfully"));
        }

        private static ProductDto MapToDto(Product product) => new()
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
