using Azure.Messaging.ServiceBus;
using Catalog.Core.Interfaces;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Messaging;
using Catalog.Infrastructure.Repositories;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Catalog.Infrastructure.Extensions
{
    public static class InfrastructureServiceExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // SQL Server
            services.AddDbContext<CatalogDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("CatalogDb")));

            // Cosmos DB
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

            // Repositories
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<CatalogDataSeeder>();

            // Event Publisher (Catalog publishes its own events)
            services.AddScoped<IEventPublisher, InMemoryEventPublisher>();

            // Messaging Consumer — swap via appsettings.json
            var provider = configuration["Messaging:Provider"] ?? "InMemory";

            switch (provider)
            {
                case "ServiceBus":
                    var sbConnection = configuration["Messaging:ServiceBus:ConnectionString"]!;
                    var topicName = configuration["Messaging:ServiceBus:TopicName"]!;
                    var subscriptionName = configuration["Messaging:ServiceBus:SubscriptionName"]!;
                    services.AddSingleton(new ServiceBusClient(sbConnection));
                    services.AddScoped<IOrderPlacedConsumer>(sp =>
                        new ServiceBusOrderPlacedConsumer(
                            sp.GetRequiredService<ServiceBusClient>(),
                            topicName,
                            subscriptionName,
                            sp.GetRequiredService<IServiceScopeFactory>(),
                            sp.GetRequiredService<ILogger<ServiceBusOrderPlacedConsumer>>()));
                    break;

                case "StorageQueue":
                    var sqConnection = configuration["Messaging:StorageQueue:ConnectionString"]!;
                    var queueName = configuration["Messaging:StorageQueue:CatalogQueueName"]!;
                    services.AddScoped<IOrderPlacedConsumer>(sp =>
                        new StorageQueueOrderPlacedConsumer(
                            sqConnection,
                            queueName,
                            sp.GetRequiredService<IServiceScopeFactory>(),
                            sp.GetRequiredService<ILogger<StorageQueueOrderPlacedConsumer>>()));
                    break;

                default: // InMemory
                    services.AddScoped<IOrderPlacedConsumer, InMemoryOrderPlacedConsumer>();
                    break;
            }

            // Background service — watches queue/topic continuously
            services.AddHostedService<OrderPlacedBackgroundService>();

            return services;
        }
    }
}