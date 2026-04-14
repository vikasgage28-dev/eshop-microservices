using EShop.Shared.DTOs;
using MediatR;

namespace EShop.Core.Features.Products.Queries
{
    public class GetProductsByCategoryQuery : IRequest<IEnumerable<ProductDto>>
    {
        public string Category { get; set; }

        public GetProductsByCategoryQuery(string category)
        {
            Category = category;
        }
    }
}