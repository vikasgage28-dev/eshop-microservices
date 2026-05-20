using MediatR;
using Ordering.Core.Entities;

namespace Ordering.Core.Features.Orders.Commands.PlaceOrder
{
    public class PlaceOrderCommand : IRequest<Order>
    {
        public string CustomerId { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string? ShippingAddress { get; set; }
        public string? Notes { get; set; }
        public List<PlaceOrderItemDto> Items { get; set; } = new();
    }
}
