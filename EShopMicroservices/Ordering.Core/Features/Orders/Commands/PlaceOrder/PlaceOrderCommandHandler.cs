using MediatR;
using Ordering.Core.Entities;
using Ordering.Core.Events;
using Ordering.Core.Interfaces;

namespace Ordering.Core.Features.Orders.Commands.PlaceOrder
{
    public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, Order>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IEventPublisher _eventPublisher;

        public PlaceOrderCommandHandler(IOrderRepository orderRepository, IEventPublisher eventPublisher)
        {
            _orderRepository = orderRepository;
            _eventPublisher = eventPublisher;
        }

        public async Task<Order> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
        {
            var order = new Order
            {
                CustomerId = request.CustomerId,
                CustomerEmail = request.CustomerEmail,
                ShippingAddress = request.ShippingAddress,
                Notes = request.Notes,
                Status = OrderStatus.Pending,
                Items = request.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    UnitPrice = i.UnitPrice,
                    Quantity = i.Quantity
                }).ToList()
            };

            var savedOrder = await _orderRepository.AddAsync(order);

            await _eventPublisher.PublishAsync(new OrderPlacedEvent(
                savedOrder.Id,
                savedOrder.CustomerId,
                savedOrder.CustomerEmail,
                savedOrder.TotalAmount));

            return savedOrder;
        }
    }
}
