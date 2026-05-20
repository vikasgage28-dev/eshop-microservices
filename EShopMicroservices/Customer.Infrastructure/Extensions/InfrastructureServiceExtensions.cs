using Customer.Core.Interfaces;
using Customer.Infrastructure.Data;
using Customer.Infrastructure.Messaging;
using Customer.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Customer.Infrastructure.Extensions
{
    public static class InfrastructureServiceExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // EF Core — SQL Server
            services.AddDbContext<CustomerDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("CustomerDb")));

            // Repositories
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            // Event Publisher (dev: in-memory, prod: swap to ServiceBus)
            services.AddScoped<IEventPublisher, InMemoryEventPublisher>();

            // Data Seeder
            services.AddScoped<CustomerDataSeeder>();

            return services;
        }
    }
}
