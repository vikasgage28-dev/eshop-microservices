using EShop.Shared.DTOs;
using MediatR;

namespace EShop.Core.Features.Products.Commands
{
    public class CreateProductCommand : IRequest<ProductDto>
    {
        // These properties = request body from client
        // Controller will bind JSON body directly to this class
        // No separate DTO needed!
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Category { get; set; } = string.Empty;
    }
}