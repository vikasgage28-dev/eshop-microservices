using MediatR;
using Ordering.Core.Entities;

namespace Ordering.Core.Features.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQuery : IRequest<Order?>
    {
        public Guid OrderId { get; set; }

        public GetOrderByIdQuery(Guid orderId) => OrderId = orderId;
    }
}
