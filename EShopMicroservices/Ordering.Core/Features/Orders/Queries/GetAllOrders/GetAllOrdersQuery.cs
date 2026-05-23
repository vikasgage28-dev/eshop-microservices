using MediatR;
using Ordering.Core.Entities;

namespace Ordering.Core.Features.Orders.Queries.GetAllOrders
{
    public class GetAllOrdersQuery : IRequest<IEnumerable<Order>>
    {
    }
}
