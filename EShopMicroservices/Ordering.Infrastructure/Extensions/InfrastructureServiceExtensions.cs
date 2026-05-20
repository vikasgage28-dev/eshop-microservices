using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Core.Interfaces;
using Ordering.Infrastructure.Data;
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
            // SQL Server — Orders + OrderItems
            services.AddDbContext<OrderingDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("OrderingDb")));

            // Repository registrations
            services.AddScoped<IOrderRepository, OrderRepository>();

            // Data seeder — called explicitly in Program.cs (Development only!)
            services.AddScoped<OrderingDataSeeder>();

            // Event publisher
            // Dev  → InMemoryEventPublisher (logs to console, no Azure needed!)
            // Prod → ServiceBusEventPublisher (Phase 13!)
            services.AddScoped<IEventPublisher, InMemoryEventPublisher>();

            return services;
        }
    }
}
