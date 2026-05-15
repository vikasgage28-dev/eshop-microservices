using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using Catalog.Infrastructure.Data;
using Microsoft.Azure.Cosmos;

namespace Catalog.Infrastructure.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly Container _container;

        public ReviewRepository(CosmosClient cosmosClient)
        {
            // Use constants from CatalogDataSeeder — single source of truth!
            _container = cosmosClient
                .GetDatabase(CatalogDataSeeder.DatabaseName)
                .GetContainer(CatalogDataSeeder.ContainerName);
        }

        public async Task<IEnumerable<Review>> GetByProductIdAsync(Guid productId)
        {
            var query = new QueryDefinition(
                "SELECT * FROM c WHERE c.productId = @productId")
                .WithParameter("@productId", productId);

            var results  = new List<Review>();
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
            // Partition key = productId (string) for efficient querying per product
            var response = await _container.CreateItemAsync(
                review,
                new PartitionKey(review.ProductId.ToString()));

            return response.Resource;
        }

        public async Task DeleteAsync(string id, Guid productId)
        {
            await _container.DeleteItemAsync<Review>(
                id,
                new PartitionKey(productId.ToString()));
        }
    }
}
