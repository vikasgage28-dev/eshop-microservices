using EShop.Core.Entities;

namespace EShop.Core.Interfaces
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetByProductIdAsync(int productId);
        Task<Review> CreateAsync(Review review);
        Task DeleteAsync(string id, int productId);
    }
}