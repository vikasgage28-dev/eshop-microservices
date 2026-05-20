using Customer.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Customer.Infrastructure.Messaging
{
    public class InMemoryEventPublisher : IEventPublisher
    {
        private readonly ILogger<InMemoryEventPublisher> _logger;

        public InMemoryEventPublisher(ILogger<InMemoryEventPublisher> logger)
        {
            _logger = logger;
        }

        public Task PublishAsync<TEvent>(TEvent @event) where TEvent : class
        {
            var eventName = typeof(TEvent).Name;
            var payload   = JsonSerializer.Serialize(@event);
            _logger.LogInformation("[EVENT PUBLISHED] {EventName}: {Payload}", eventName, payload);
            return Task.CompletedTask;
        }
    }
}
