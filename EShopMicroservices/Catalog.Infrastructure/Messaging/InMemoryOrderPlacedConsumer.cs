using Catalog.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Catalog.Infrastructure.Messaging
{
    /// <summary>
    /// DEV ONLY — No real queue. Just logs that it's waiting.
    /// InMemoryEventPublisher (Ordering) logs the event.
    /// This consumer sleeps — no cross-process messaging locally.
    /// Switch to StorageQueue for real cross-service messaging.
    /// </summary>
    public class InMemoryOrderPlacedConsumer : IOrderPlacedConsumer
    {
        private readonly ILogger<InMemoryOrderPlacedConsumer> _logger;

        public InMemoryOrderPlacedConsumer(
            ILogger<InMemoryOrderPlacedConsumer> logger)
        {
            _logger = logger;
        }

        public async Task ConsumeAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "[IN-MEMORY CONSUMER] Running in dev mode. " +
                "No real queue — switch to StorageQueue for cross-service messaging.");

            // Nothing to consume — just wait until app stops
            await Task.Delay(Timeout.Infinite, cancellationToken);
        }
    }
}