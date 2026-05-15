using Catalog.Core.Entities;

namespace Catalog.Core.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(Guid id);
        Task<Category> CreateAsync(Category category);
        Task<Category?> UpdateAsync(Guid id, Category category);
        Task<bool> DeleteAsync(Guid id);
    }
}
