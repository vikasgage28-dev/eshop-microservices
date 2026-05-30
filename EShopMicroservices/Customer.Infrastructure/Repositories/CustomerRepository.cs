using Customer.Core.Entities;
using Customer.Core.Interfaces;
using Customer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Customer.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerDbContext _context;

        public CustomerRepository(CustomerDbContext context)
        {
            _context = context;
        }

        public async Task<Core.Entities.Customer?> GetByIdAsync(Guid id)
            => await _context.Customers
                .Include(c => c.Addresses)
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task<Core.Entities.Customer?> GetByEmailAsync(string email)
            => await _context.Customers
                .Include(c => c.Addresses)
                .FirstOrDefaultAsync(c => c.Email.ToLower() == email.ToLower());

        public async Task<IEnumerable<Core.Entities.Customer>> GetAllAsync()
            => await _context.Customers
                .Include(c => c.Addresses)
                .OrderBy(c => c.LastName).ThenBy(c => c.FirstName)
                .ToListAsync();

        public async Task<Core.Entities.Customer> AddAsync(Core.Entities.Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<Core.Entities.Customer?> UpdateAsync(Core.Entities.Customer customer)
        {
            var existing = await _context.Customers.FindAsync(customer.Id);
            if (existing is null) return null;

            _context.Entry(existing).CurrentValues.SetValues(customer);
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer is null) return false;

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Address?> AddAddressAsync(Guid customerId, Address address)
        {
            var exists = await _context.Customers.AnyAsync(c => c.Id == customerId);
            if (!exists) return null;

            address.CustomerId = customerId;
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();
            return address;
        }

        public async Task<bool> DeleteAddressAsync(Guid customerId, Guid addressId)
        {
            var address = await _context.Addresses
                .FirstOrDefaultAsync(a => a.Id == addressId && a.CustomerId == customerId);
            if (address is null) return false;

            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
