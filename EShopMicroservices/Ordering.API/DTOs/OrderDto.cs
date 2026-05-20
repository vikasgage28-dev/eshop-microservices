using Ordering.Core.Entities;

namespace Ordering.API.DTOs
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string CustomerId { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public OrderStatus Status { get; set; }
        public string StatusName => Status.ToString();
        public DateTime OrderDate { get; set; }
        public string? ShippingAddress { get; set; }
        public string? Notes { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }
}
