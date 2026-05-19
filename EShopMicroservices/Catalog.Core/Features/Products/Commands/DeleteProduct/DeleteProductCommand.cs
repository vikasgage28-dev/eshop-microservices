using MediatR;

namespace Catalog.Core.Features.Products.Commands.DeleteProduct
{
    public record DeleteProductCommand(Guid Id) : IRequest<bool>;
}
