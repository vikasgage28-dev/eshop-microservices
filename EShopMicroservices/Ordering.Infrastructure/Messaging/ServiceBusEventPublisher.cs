using Microsoft.Extensions.Logging;
using Ordering.Core.Interfaces;

namespace Ordering.Infrastructure.Messaging
{
    /// <summary>
    /// Production implementation — publishes to Azure Service Bus.
    /// Wired in Phase 13 (CI/CD + Azure)!
    /// </summary>
    public class ServiceBusEventPublisher : IEventPublisher
    {
        private readonly ILogger<ServiceBusEventPublisher> _logger;

        public ServiceBusEventPublisher(ILogger<ServiceBusEventPublisher> logger)
        {
            _logger = logger;
        }

        public Task PublishAsync<TEvent>(TEvent @event) where TEvent : class
        {
            // TODO Phase 13: send to Azure Service Bus topic
            _logger.LogWarning(
                "[SERVICE BUS STUB] Would publish {EventType} to Service Bus.",
                typeof(TEvent).Name);

            return Task.CompletedTask;
        }
    }
}
