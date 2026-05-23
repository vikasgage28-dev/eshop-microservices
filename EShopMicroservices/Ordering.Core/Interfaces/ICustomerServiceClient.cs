namespace Ordering.Core.Interfaces
{
    public class CustomerDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public interface ICustomerServiceClient
    {
        Task<CustomerDto?> GetCustomerByIdAsync(Guid customerId);
    }
}