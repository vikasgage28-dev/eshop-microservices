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
        private readonly ICustomerServiceClient _customerServiceClient;

        public PlaceOrderCommandHandler(
            IOrderRepository orderRepository,
            IEventPublisher eventPublisher,
            ICustomerServiceClient customerServiceClient)
        {
            _orderRepository = orderRepository;
            _eventPublisher = eventPublisher;
            _customerServiceClient = customerServiceClient;
        }

        public async Task<Order> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
        {
            // Validate customer exists in Customer.API
            var customer = await _customerServiceClient
                .GetCustomerByIdAsync(Guid.Parse(request.CustomerId));

            if (customer is null)
                throw new KeyNotFoundException(
                    $"Customer {request.CustomerId} not found.");

            var order = new Order
            {
                CustomerId = request.CustomerId,
                CustomerEmail = customer.Email,
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