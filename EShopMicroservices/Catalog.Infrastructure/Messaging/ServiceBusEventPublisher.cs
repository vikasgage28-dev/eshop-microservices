using Catalog.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Catalog.Infrastructure.Messaging
{
    /// <summary>
    /// Production implementation of IEventPublisher.
    /// Sends events to Azure Service Bus.
    /// Requires "ServiceBus:ConnectionString" in appsettings.Production.json
    /// Activated in Phase 13 (CI/CD pipeline)!
    /// </summary>
    public class ServiceBusEventPublisher : IEventPublisher
    {
        private readonly ILogger<ServiceBusEventPublisher> _logger;
        private readonly string _connectionString;

        public ServiceBusEventPublisher(
            ILogger<ServiceBusEventPublisher> logger,
            string connectionString)
        {
            _logger           = logger;
            _connectionString = connectionString;
        }

        public async Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default)
            where T : class
        {
            // Phase 13: Wire real Azure.Messaging.ServiceBus client here!
            // var client = new ServiceBusClient(_connectionString);
            // var sender = client.CreateSender(typeof(T).Name.ToLower());
            // var message = new ServiceBusMessage(JsonSerializer.Serialize(@event));
            // await sender.SendMessageAsync(message, cancellationToken);

            var eventName = typeof(T).Name;
            _logger.LogInformation(
                "[SERVICE BUS] Would publish {EventName} to Azure Service Bus (Phase 13!)",
                eventName);

            await Task.CompletedTask;
        }
    }
}
