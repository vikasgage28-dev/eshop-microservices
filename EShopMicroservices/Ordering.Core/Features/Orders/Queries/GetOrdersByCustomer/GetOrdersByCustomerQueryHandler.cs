using MediatR;
using Ordering.Core.Entities;
using Ordering.Core.Interfaces;

namespace Ordering.Core.Features.Orders.Queries.GetOrdersByCustomer
{
    public class GetOrdersByCustomerQueryHandler : IRequestHandler<GetOrdersByCustomerQuery, IEnumerable<Order>>
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrdersByCustomerQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<Order>> Handle(GetOrdersByCustomerQuery request, CancellationToken cancellationToken)
        {
            return await _orderRepository.GetByCustomerIdAsync(request.CustomerId);
        }
    }
}
