using Catalog.Core.Interfaces;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Repositories;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Infrastructure.Extensions
{
    public static class InfrastructureServiceExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // SQL Server — Products + Categories
            services.AddDbContext<CatalogDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("CatalogDb")));

            // Cosmos DB — Reviews
            // Use System.Text.Json serializer so [JsonPropertyName] attributes are respected
            // This pairs with AzureCosmosDisableNewtonsoftJsonCheck=true in the csproj
            var cosmosConnectionString = configuration.GetConnectionString("CosmosDb")!;
            var cosmosOptions = new CosmosClientOptions
            {
                Serializer = new CosmosSystemTextJsonSerializer(
                    new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
                    })
            };
            services.AddSingleton(new CosmosClient(cosmosConnectionString, cosmosOptions));

            // Repository registrations
            services.AddScoped<IProductRepository,  ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IReviewRepository,   ReviewRepository>();

            return services;
        }
    }
}
