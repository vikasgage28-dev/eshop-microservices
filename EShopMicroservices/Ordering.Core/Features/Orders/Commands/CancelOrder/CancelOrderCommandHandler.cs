using MediatR;
using Ordering.Core.Entities;
using Ordering.Core.Events;
using Ordering.Core.Interfaces;

namespace Ordering.Core.Features.Orders.Commands.CancelOrder
{
    public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IEventPublisher _eventPublisher;

        public CancelOrderCommandHandler(IOrderRepository orderRepository, IEventPublisher eventPublisher)
        {
            _orderRepository = orderRepository;
            _eventPublisher = eventPublisher;
        }

        public async Task<bool> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId);
            if (order is null) return false;

            order.Status = OrderStatus.Cancelled;
            await _orderRepository.UpdateAsync(order);

            await _eventPublisher.PublishAsync(new OrderCancelledEvent(
                order.Id,
                order.CustomerId,
                request.Reason));

            return true;
        }
    }
}
