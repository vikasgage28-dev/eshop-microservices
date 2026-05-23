using MediatR;
using Ordering.Core.Entities;

namespace Ordering.Core.Features.Orders.Queries.GetOrdersByCustomer
{
    public class GetOrdersByCustomerQuery : IRequest<IEnumerable<Order>>
    {
        public string CustomerId { get; set; }

        public GetOrdersByCustomerQuery(string customerId) => CustomerId = customerId;
    }
}
