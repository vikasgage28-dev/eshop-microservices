using Catalog.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Catalog.Infrastructure.Messaging
{
    /// <summary>
    /// Development implementation of IEventPublisher.
    /// Logs events to console instead of sending to Azure Service Bus.
    /// Swap for ServiceBusEventPublisher in Production!
    /// </summary>
    public class InMemoryEventPublisher : IEventPublisher
    {
        private readonly ILogger<InMemoryEventPublisher> _logger;

        public InMemoryEventPublisher(ILogger<InMemoryEventPublisher> logger)
        {
            _logger = logger;
        }

        public Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default)
            where T : class
        {
            var eventName    = typeof(T).Name;
            var eventPayload = JsonSerializer.Serialize(@event, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            _logger.LogInformation(
                "[EVENT PUBLISHED] {EventName}\n{Payload}",
                eventName,
                eventPayload);

            return Task.CompletedTask;
        }
    }
}
