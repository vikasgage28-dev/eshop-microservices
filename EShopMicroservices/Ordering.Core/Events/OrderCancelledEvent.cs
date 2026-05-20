namespace Ordering.Core.Events
{
    public class OrderCancelledEvent
    {
        public Guid OrderId { get; }
        public string CustomerId { get; }
        public string Reason { get; }
        public DateTime OccurredAt { get; } = DateTime.UtcNow;

        public OrderCancelledEvent(Guid orderId, string customerId, string reason)
        {
            OrderId = orderId;
            CustomerId = customerId;
            Reason = reason;
        }
    }
}
