using Microsoft.Extensions.Logging;
using Ordering.Core.Interfaces;

namespace Ordering.Infrastructure.Messaging
{
    /// <summary>
    /// Development implementation — logs events to console.
    /// No Azure Service Bus required locally!
    /// </summary>
    public class InMemoryEventPublisher : IEventPublisher
    {
        private readonly ILogger<InMemoryEventPublisher> _logger;

        public InMemoryEventPublisher(ILogger<InMemoryEventPublisher> logger)
        {
            _logger = logger;
        }

        public Task PublishAsync<TEvent>(TEvent @event) where TEvent : class
        {
            _logger.LogInformation(
                "[EVENT PUBLISHED] {EventType}: {EventData}",
                typeof(TEvent).Name,
                System.Text.Json.JsonSerializer.Serialize(@event));

            return Task.CompletedTask;
        }
    }
}
