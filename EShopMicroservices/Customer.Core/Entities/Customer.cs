namespace Customer.Core.Entities
{
    public class Customer
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Computed
        public string FullName => $"{FirstName} {LastName}".Trim();

        // Navigation
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
    }
}
