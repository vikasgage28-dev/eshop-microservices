using Catalog.Core.Entities;

namespace Catalog.Core.Interfaces
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetByProductIdAsync(Guid productId);
        Task<Review> CreateAsync(Review review);
        Task DeleteAsync(string id, Guid productId);
    }
}
