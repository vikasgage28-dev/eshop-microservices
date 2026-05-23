using Catalog.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Catalog.Infrastructure.Messaging
{
    public class OrderPlacedBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<OrderPlacedBackgroundService> _logger;

        public OrderPlacedBackgroundService(
            IServiceScopeFactory scopeFactory,
            ILogger<OrderPlacedBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "[BACKGROUND SERVICE] OrderPlacedBackgroundService starting...");

            using var scope = _scopeFactory.CreateScope();
            var consumer = scope.ServiceProvider
                .GetRequiredService<IOrderPlacedConsumer>();

            await consumer.ConsumeAsync(stoppingToken);

            _logger.LogInformation(
                "[BACKGROUND SERVICE] OrderPlacedBackgroundService stopped.");
        }
    }
}