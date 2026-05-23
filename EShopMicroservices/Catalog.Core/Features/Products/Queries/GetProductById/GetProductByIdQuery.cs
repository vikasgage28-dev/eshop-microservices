using Catalog.Core.Entities;
using MediatR;

namespace Catalog.Core.Features.Products.Queries.GetProductById
{
    public record GetProductByIdQuery(Guid Id) : IRequest<Product?>;
}
