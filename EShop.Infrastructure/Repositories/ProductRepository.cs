using EShop.Core.Entities;
using EShop.Core.Interfaces;

namespace EShop.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private static List<Product> _products = new()
        {
            new Product { Id = 1, Name = "Laptop", Description = "High performance laptop", Price = 999.99m, Stock = 10, Category = "Electronics", CreatedAt = DateTime.UtcNow },
            new Product { Id = 2, Name = "Wireless Mouse", Description = "Ergonomic wireless mouse", Price = 29.99m, Stock = 50, Category = "Electronics", CreatedAt = DateTime.UtcNow },
            new Product { Id = 3, Name = "Standing Desk", Description = "Adjustable standing desk", Price = 499.99m, Stock = 5, Category = "Furniture", CreatedAt = DateTime.UtcNow },
            new Product { Id = 4, Name = "Mechanical Keyboard", Description = "RGB mechanical keyboard", Price = 149.99m, Stock = 20, Category = "Electronics", CreatedAt = DateTime.UtcNow },
            new Product { Id = 5, Name = "Office Chair", Description = "Ergonomic office chair", Price = 299.99m, Stock = 8, Category = "Furniture", CreatedAt = DateTime.UtcNow },
        };

        private static int _nextId = 6;

        public Task<IEnumerable<Product>> GetAllAsync()
        {
            return Task.FromResult(_products.Where(p => p.IsActive).AsEnumerable());
        }

        public Task<Product?> GetByIdAsync(int id)
        {
            return Task.FromResult(_products.FirstOrDefault(p => p.Id == id));
        }

        public Task<IEnumerable<Product>> GetByCategoryAsync(string category)
        {
            var result = _products
                .Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase) && p.IsActive)
                .AsEnumerable();
            return Task.FromResult(result);
        }

        public Task<Product> CreateAsync(Product product)
        {
            product.Id = _nextId++;
            product.CreatedAt = DateTime.UtcNow;
            _products.Add(product);
            return Task.FromResult(product);
        }

        public Task<Product?> UpdateAsync(int id, Product product)
        {
            var existing = _products.FirstOrDefault(p => p.Id == id);
            if (existing == null) return Task.FromResult<Product?>(null);

            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.Price = product.Price;
            existing.Stock = product.Stock;
            existing.Category = product.Category;
            existing.IsActive = product.IsActive;

            return Task.FromResult<Product?>(existing);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null) return Task.FromResult(false);

            product.IsActive = false; // Soft delete
            return Task.FromResult(true);
        }
    }
}
