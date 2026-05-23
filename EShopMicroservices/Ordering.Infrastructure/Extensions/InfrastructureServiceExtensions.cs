using Azure.Messaging.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ordering.Core.Interfaces;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.HttpClients;
using Ordering.Infrastructure.Messaging;
using Ordering.Infrastructure.Repositories;

namespace Ordering.Infrastructure.Extensions
{
    public static class InfrastructureServiceExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // SQL Server
            services.AddDbContext<OrderingDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("OrderingDb")));

            // Repositories
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<OrderingDataSeeder>();

            // Messaging — swap provider via appsettings.json
            var provider = configuration["Messaging:Provider"] ?? "InMemory";

            switch (provider)
            {
                case "ServiceBus":
                    var sbConnection = configuration["Messaging:ServiceBus:ConnectionString"]!;
                    var topicName = configuration["Messaging:ServiceBus:TopicName"]!;
                    services.AddSingleton(new ServiceBusClient(sbConnection));
                    services.AddScoped<IEventPublisher>(sp =>
                        new ServiceBusEventPublisher(
                            sp.GetRequiredService<ServiceBusClient>(),
                            topicName,
                            sp.GetRequiredService<ILogger<ServiceBusEventPublisher>>()));
                    break;

                case "StorageQueue":
                    var sqConnection = configuration["Messaging:StorageQueue:ConnectionString"]!;
                    var queueNames = configuration
                                                 .GetSection("Messaging:StorageQueue:QueueNames")
                                                 .GetChildren()
                                                 .Select(x => x.Value!)
                                                 .ToList();
                    services.AddScoped<IEventPublisher>(sp =>
                        new StorageQueueEventPublisher(
                            sqConnection,
                            queueNames,
                            sp.GetRequiredService<ILogger<StorageQueueEventPublisher>>()));
                    break;

                default: // InMemory
                    services.AddScoped<IEventPublisher, InMemoryEventPublisher>();
                    break;
            }

            return services;
        }
    }
}