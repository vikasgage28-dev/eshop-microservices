using Catalog.Core.Entities;
using Catalog.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Catalog.Infrastructure.Data
{
    // Runs at startup in Development to populate sample data.
    // NOT a migration — business data belongs here, not in HasData()!
    public class CatalogDataSeeder
    {
        private readonly CatalogDbContext _context;
        private readonly ILogger<CatalogDataSeeder> _logger;

        public CatalogDataSeeder(CatalogDbContext context, ILogger<CatalogDataSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            // Apply any pending migrations automatically
            await _context.Database.MigrateAsync();

            await SeedCategoriesAsync();
            await SeedProductsAsync();
        }

        private async Task SeedCategoriesAsync()
        {
            if (await _context.Categories.AnyAsync())
            {
                _logger.LogInformation("Categories already seeded — skipping.");
                return;
            }

            var categories = new List<Category>
            {
                new() { Id = CategoryConfiguration.ElectronicsId, Name = "Electronics", Description = "Electronic devices and accessories" },
                new() { Id = CategoryConfiguration.FurnitureId,   Name = "Furniture",   Description = "Home and office furniture" }
            };

            _context.Categories.AddRange(categories);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Seeded {Count} categories.", categories.Count);
        }

        private async Task SeedProductsAsync()
        {
            if (await _context.Products.AnyAsync())
            {
                _logger.LogInformation("Products already seeded — skipping.");
                return;
            }

            var products = new List<Product>
            {
                new() { Name = "Laptop",             Description = "High performance laptop",   Price = 999.99m, Stock = 10, CategoryId = CategoryConfiguration.ElectronicsId },
                new() { Name = "Wireless Mouse",     Description = "Ergonomic wireless mouse",  Price = 29.99m,  Stock = 50, CategoryId = CategoryConfiguration.ElectronicsId },
                new() { Name = "Mechanical Keyboard",Description = "RGB mechanical keyboard",   Price = 149.99m, Stock = 20, CategoryId = CategoryConfiguration.ElectronicsId },
                new() { Name = "Standing Desk",      Description = "Adjustable standing desk",  Price = 499.99m, Stock = 5,  CategoryId = CategoryConfiguration.FurnitureId   },
                new() { Name = "Office Chair",       Description = "Ergonomic office chair",    Price = 299.99m, Stock = 8,  CategoryId = CategoryConfiguration.FurnitureId   }
            };

            _context.Products.AddRange(products);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Seeded {Count} products.", products.Count);
        }
    }
}
