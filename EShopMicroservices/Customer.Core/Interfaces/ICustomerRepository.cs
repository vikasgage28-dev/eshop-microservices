using Customer.Core.Entities;

namespace Customer.Core.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Entities.Customer?> GetByIdAsync(Guid id);
        Task<Entities.Customer?> GetByEmailAsync(string email);
        Task<IEnumerable<Entities.Customer>> GetAllAsync();
        Task<Entities.Customer> AddAsync(Entities.Customer customer);
        Task<Entities.Customer?> UpdateAsync(Entities.Customer customer);
        Task<bool> DeleteAsync(Guid id);
    }
}
