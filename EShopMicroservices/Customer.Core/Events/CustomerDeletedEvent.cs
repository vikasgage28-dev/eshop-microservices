namespace Customer.Core.Events
{
    public class CustomerDeletedEvent
    {
        public Guid CustomerId { get; set; }
        public string Email { get; set; } = string.Empty;
        public DateTime DeletedAt { get; set; }
    }
}
