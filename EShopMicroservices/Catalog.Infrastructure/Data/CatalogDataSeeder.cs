using Catalog.Core.Entities;
using Catalog.Infrastructure.Data.Configurations;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Catalog.Infrastructure.Data
{
    // Runs at startup in Development to populate sample data.
    // NOT a migration — business data belongs here, not in HasData()!
    public class CatalogDataSeeder
    {
        private readonly CatalogDbContext _context;
        private readonly CosmosClient _cosmosClient;
        private readonly ILogger<CatalogDataSeeder> _logger;

        // Cosmos DB names — single source of truth!
        public const string DatabaseName  = "CatalogDb";
        public const string ContainerName = "reviews";
        public const string PartitionKey  = "/productId";

        public CatalogDataSeeder(
            CatalogDbContext context,
            CosmosClient cosmosClient,
            ILogger<CatalogDataSeeder> logger)
        {
            _context      = context;
            _cosmosClient = cosmosClient;
            _logger       = logger;
        }

        public async Task SeedAsync()
        {
            // Note: MigrateAsync() is called in Program.cs before SeedAsync()
            // No need to call it here again!

            // Cosmos DB — create database + container if not exists
            // Wrapped in try-catch so app works even without Cosmos Emulator locally!
            await InitializeCosmosAsync();

            await SeedCategoriesAsync();
            await SeedProductsAsync();
        }

        private async Task InitializeCosmosAsync()
        {
            try
            {
                // CreateDatabaseIfNotExistsAsync → safe to call every startup!
                var dbResponse = await _cosmosClient
                    .CreateDatabaseIfNotExistsAsync(DatabaseName);

                // CreateContainerIfNotExistsAsync → safe to call every startup!
                // Partition key = /productId → all reviews for a product in same partition
                await dbResponse.Database.CreateContainerIfNotExistsAsync(
                    new ContainerProperties(ContainerName, PartitionKey));

                _logger.LogInformation(
                    "Cosmos DB '{Database}' and container '{Container}' ready.",
                    DatabaseName, ContainerName);
            }
            catch (Exception ex)
            {
                // Cosmos Emulator not running locally — that's OK!
                // Reviews won't work but Products + Categories will!
                _logger.LogWarning(
                    "Cosmos DB unavailable — Reviews disabled. " +
                    "Start Cosmos Emulator for full functionality. Error: {Message}",
                    ex.Message);
            }
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
