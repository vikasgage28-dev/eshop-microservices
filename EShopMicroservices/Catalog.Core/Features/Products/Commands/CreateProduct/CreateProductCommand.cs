using Catalog.Core.Entities;
using MediatR;

namespace Catalog.Core.Features.Products.Commands.CreateProduct
{
    public record CreateProductCommand(
        string  Name,
        string  Description,
        decimal Price,
        int     Stock,
        Guid    CategoryId
    ) : IRequest<Product>;
}
