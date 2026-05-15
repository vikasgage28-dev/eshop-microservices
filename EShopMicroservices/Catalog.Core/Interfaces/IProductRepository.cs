using Catalog.Core.Entities;

namespace Catalog.Core.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(Guid id);
        Task<IEnumerable<Product>> GetByCategoryAsync(Guid categoryId);
        Task<Product> CreateAsync(Product product);
        Task<Product?> UpdateAsync(Guid id, Product product);
        Task<bool> DeleteAsync(Guid id);
        Task<(IEnumerable<Product> Products, int TotalCount)> GetPagedAsync(
            int page,
            int pageSize,
            string? search = null,
            Guid? categoryId = null);
    }
}
