using Customer.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Customer.Infrastructure.Data
{
    public class CustomerDataSeeder
    {
        private readonly CustomerDbContext _context;
        private readonly ILogger<CustomerDataSeeder> _logger;

        public CustomerDataSeeder(CustomerDbContext context, ILogger<CustomerDataSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            // Idempotent — only seed if no customers exist
            if (await _context.Customers.AnyAsync())
            {
                _logger.LogInformation("Customer data already seeded. Skipping.");
                return;
            }

            var customers = new List<Core.Entities.Customer>
            {
                new()
                {
                    Id        = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    FirstName = "Alice",
                    LastName  = "Smith",
                    Email     = "alice@eshop.com",   // matches Identity seeded email
                    Phone     = "+91-9000000001",
                    CreatedAt = DateTime.UtcNow,
                    Addresses = new List<Address>
                    {
                        new()
                        {
                            Street     = "123 MG Road",
                            City       = "Mumbai",
                            State      = "Maharashtra",
                            Country    = "India",
                            PostalCode = "400001",
                            IsDefault  = true
                        }
                    }
                },
                new()
                {
                    Id        = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    FirstName = "Bob",
                    LastName  = "Jones",
                    Email     = "bob.jones@eshop.com",
                    Phone     = "+91-9000000002",
                    CreatedAt = DateTime.UtcNow,
                    Addresses = new List<Address>
                    {
                        new()
                        {
                            Street     = "456 Brigade Road",
                            City       = "Bengaluru",
                            State      = "Karnataka",
                            Country    = "India",
                            PostalCode = "560001",
                            IsDefault  = true
                        }
                    }
                }
            };

            await _context.Customers.AddRangeAsync(customers);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Seeded {Count} customers successfully.", customers.Count);
        }
    }
}
