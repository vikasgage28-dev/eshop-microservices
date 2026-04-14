using EShop.Shared.DTOs;
using MediatR;

namespace EShop.Core.Features.Products.Queries
{
    public class GetAllProductsQuery : IRequest<IEnumerable<ProductDto>>
    {
        // Empty class
        // No properties needed
        // Just tells MediatR "I want all products"
        // IRequest<T> = what we expect BACK in response
    }
}