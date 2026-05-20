using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data
{
    public class OrderingDataSeeder
    {
        private readonly OrderingDbContext _context;

        public OrderingDataSeeder(OrderingDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            // Idempotent — only seed if no orders exist
            if (await _context.Orders.AnyAsync()) return;

            var order1 = new Order
            {
                CustomerId = "customer-001",
                CustomerEmail = "alice@eshop.com",
                Status = OrderStatus.Delivered,
                ShippingAddress = "123 Main St, Springfield, IL 62701",
                OrderDate = DateTime.UtcNow.AddDays(-10),
                Items = new List<OrderItem>
                {
                    new OrderItem
                    {
                        ProductName = "Laptop Pro 15",
                        ProductId = Guid.NewGuid(),
                        UnitPrice = 1299.99m,
                        Quantity = 1
                    },
                    new OrderItem
                    {
                        ProductName = "Wireless Mouse",
                        ProductId = Guid.NewGuid(),
                        UnitPrice = 49.99m,
                        Quantity = 2
                    }
                }
            };

            var order2 = new Order
            {
                CustomerId = "customer-002",
                CustomerEmail = "bob@eshop.com",
                Status = OrderStatus.Processing,
                ShippingAddress = "456 Oak Ave, Chicago, IL 60601",
                OrderDate = DateTime.UtcNow.AddDays(-2),
                Items = new List<OrderItem>
                {
                    new OrderItem
                    {
                        ProductName = "Mechanical Keyboard",
                        ProductId = Guid.NewGuid(),
                        UnitPrice = 149.99m,
                        Quantity = 1
                    }
                }
            };

            await _context.Orders.AddRangeAsync(order1, order2);
            await _context.SaveChangesAsync();
        }
    }
}
