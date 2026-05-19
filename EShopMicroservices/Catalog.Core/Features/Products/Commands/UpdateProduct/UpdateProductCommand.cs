using Catalog.Core.Entities;
using MediatR;

namespace Catalog.Core.Features.Products.Commands.UpdateProduct
{
    public record UpdateProductCommand(
        Guid    Id,
        string  Name,
        string  Description,
        decimal Price,
        int     Stock,
        Guid    CategoryId,
        bool    IsActive
    ) : IRequest<Product?>;
}
