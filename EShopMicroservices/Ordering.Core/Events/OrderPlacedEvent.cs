namespace Ordering.Core.Events
{
    public class OrderPlacedEvent
    {
        public Guid OrderId { get; }
        public string CustomerId { get; }
        public string CustomerEmail { get; }
        public decimal TotalAmount { get; }
        public DateTime OccurredAt { get; } = DateTime.UtcNow;

        public OrderPlacedEvent(Guid orderId, string customerId, string customerEmail, decimal totalAmount)
        {
            OrderId = orderId;
            CustomerId = customerId;
            CustomerEmail = customerEmail;
            TotalAmount = totalAmount;
        }
    }
}
