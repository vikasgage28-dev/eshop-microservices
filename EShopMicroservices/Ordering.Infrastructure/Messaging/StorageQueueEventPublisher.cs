using Azure.Storage.Queues;
using Microsoft.Extensions.Logging;
using Ordering.Core.Interfaces;
using System.Text;
using System.Text.Json;

namespace Ordering.Infrastructure.Messaging
{
    /// <summary>
    /// STORAGE QUEUE IMPLEMENTATION — Long-term deployment (9 months).
    /// Manual fan-out: publishes to ONE queue per consumer.
    /// Cost: ~₹0.03/month for our usage.
    /// Switch via appsettings.json: "Messaging:Provider": "StorageQueue"
    /// </summary>
    public class StorageQueueEventPublisher : IEventPublisher
    {
        private readonly string _connectionString;
        private readonly IEnumerable<string> _queueNames;
        private readonly ILogger<StorageQueueEventPublisher> _logger;

        public StorageQueueEventPublisher(
            string connectionString,
            IEnumerable<string> queueNames,
            ILogger<StorageQueueEventPublisher> logger)
        {
            _connectionString = connectionString;
            _queueNames = queueNames;
            _logger = logger;
        }

        public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : class
        {
            var body = JsonSerializer.Serialize(@event);
            var encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(body));

            foreach (var queueName in _queueNames)
            {
                var client = new QueueClient(_connectionString, queueName);
                await client.CreateIfNotExistsAsync();
                await client.SendMessageAsync(encoded);

                _logger.LogInformation(
                    "[STORAGE QUEUE] Published {EventType} to queue '{Queue}'",
                    typeof(TEvent).Name, queueName);
            }
        }
    }
}