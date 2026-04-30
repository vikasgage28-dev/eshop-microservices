using EShop.Core.Entities;
using EShop.Core.Interfaces;
using Microsoft.Azure.Cosmos;

namespace EShop.Infrastructure.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly Microsoft.Azure.Cosmos.Container _container;

        public ReviewRepository(CosmosClient cosmosClient)
        {
            _container = cosmosClient
                .GetDatabase("EShopDb")
                .GetContainer("reviews");
        }

        public async Task<IEnumerable<Review>> GetByProductIdAsync(int productId)
        {
            var query = new QueryDefinition(
                "SELECT * FROM c WHERE c.productId = @productId")
                .WithParameter("@productId", productId);

            var results = new List<Review>();
            var iterator = _container.GetItemQueryIterator<Review>(query);

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }

        public async Task<Review> CreateAsync(Review review)
        {
            var response = await _container.CreateItemAsync(
                review,
                new PartitionKey(review.ProductId));
            return response.Resource;
        }

        public async Task DeleteAsync(string id, int productId)
        {
            await _container.DeleteItemAsync<Review>(
                id,
                new PartitionKey(productId));
        }
    }
}