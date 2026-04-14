using EShop.Shared.DTOs;
using MediatR;

namespace EShop.Core.Features.Products.Queries
{
    public class GetProductByIdQuery : IRequest<ProductDto?>
    {
        public int Id { get; set; }

        // Constructor carries the Id
        // Controller will pass Id here
        public GetProductByIdQuery(int id)
        {
            Id = id;
        }
    }
}