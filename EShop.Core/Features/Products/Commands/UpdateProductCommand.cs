using EShop.Shared.DTOs;
using MediatR;

namespace EShop.Core.Features.Products.Commands
{
    public class UpdateProductCommand : IRequest<ProductDto?>
    {
        // Id comes from URL route: PUT /api/Products/{id}
        // Controller will set this manually
        public int Id { get; set; }

        // These come from request body
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Category { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}