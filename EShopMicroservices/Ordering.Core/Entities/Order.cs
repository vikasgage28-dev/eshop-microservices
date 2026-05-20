namespace Ordering.Core.Entities
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string CustomerId { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public string? ShippingAddress { get; set; }
        public string? Notes { get; set; }

        // Computed
        public decimal TotalAmount => Items.Sum(i => i.TotalPrice);

        // Navigation
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
