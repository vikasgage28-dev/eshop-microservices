using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using Ordering.Core.Interfaces;
using System.Text.Json;

namespace Ordering.Infrastructure.Messaging
{
    /// <summary>
    /// SERVICE BUS IMPLEMENTATION — For learning purposes.
    /// Uses Azure Service Bus Topic + Subscriptions (Standard tier).
    /// Switch via appsettings.json: "Messaging:Provider": "ServiceBus"
    /// Requires: "Messaging:ServiceBus:ConnectionString" and "Messaging:ServiceBus:TopicName"
    /// </summary>
    public class ServiceBusEventPublisher : IEventPublisher
    {
        private readonly ServiceBusClient _client;
        private readonly string _topicName;
        private readonly ILogger<ServiceBusEventPublisher> _logger;

        public ServiceBusEventPublisher(
            ServiceBusClient client,
            string topicName,
            ILogger<ServiceBusEventPublisher> logger)
        {
            _client = client;
            _topicName = topicName;
            _logger = logger;
        }

        public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : class
        {
            var sender = _client.CreateSender(_topicName);
            var body = JsonSerializer.Serialize(@event);
            var message = new ServiceBusMessage(body)
            {
                Subject = typeof(TEvent).Name,
                ContentType = "application/json",
                MessageId = Guid.NewGuid().ToString()
            };

            await sender.SendMessageAsync(message);

            _logger.LogInformation(
                "[SERVICE BUS] Published {EventType} to topic '{Topic}'",
                typeof(TEvent).Name, _topicName);
        }
    }
}