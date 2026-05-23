using Azure.Storage.Queues;
using Catalog.Core.Interfaces;
using EShop.Contracts.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace Catalog.Infrastructure.Messaging
{
    /// <summary>
    /// STORAGE QUEUE CONSUMER — Long-term deployment (9 months).
    /// Polls Azure Storage Queue for OrderPlaced events.
    /// Cost: ~₹0.03/month. Switch via "Messaging:Provider": "StorageQueue"
    /// </summary>
    public class StorageQueueOrderPlacedConsumer : IOrderPlacedConsumer
    {
        private readonly string _connectionString;
        private readonly string _queueName;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<StorageQueueOrderPlacedConsumer> _logger;

        public StorageQueueOrderPlacedConsumer(
            string connectionString,
            string queueName,
            IServiceScopeFactory scopeFactory,
            ILogger<StorageQueueOrderPlacedConsumer> logger)
        {
            _connectionString = connectionString;
            _queueName = queueName;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task ConsumeAsync(CancellationToken cancellationToken)
        {
            var client = new QueueClient(_connectionString, _queueName);
            await client.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

            _logger.LogInformation(
                "[STORAGE QUEUE] Listening on queue '{Queue}'", _queueName);

            while (!cancellationToken.IsCancellationRequested)
            {
                var response = await client.ReceiveMessagesAsync(
                    maxMessages: 10,
                    cancellationToken: cancellationToken);

                foreach (var message in response.Value)
                {
                    try
                    {
                        var body = Encoding.UTF8.GetString(
                            Convert.FromBase64String(message.MessageText));
                        var event_ = JsonSerializer.Deserialize<OrderPlacedEvent>(body);

                        if (event_ != null)
                        {
                            _logger.LogInformation(
                                "[STORAGE QUEUE] Received OrderPlaced for Order {OrderId}",
                                event_.OrderId);

                            await ReduceStockAsync(event_);
                        }

                        // Delete message from queue — processed successfully!
                        await client.DeleteMessageAsync(
                            message.MessageId,
                            message.PopReceipt,
                            cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex,
                            "[STORAGE QUEUE] Failed to process message {MessageId}",
                            message.MessageId);
                        // Message stays in queue → retried automatically!
                    }
                }

                // Wait 5 seconds before polling again
                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            }
        }

        private async Task ReduceStockAsync(OrderPlacedEvent @event)
        {
            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider
                .GetRequiredService<IProductRepository>();

            foreach (var item in @event.Items)
            {
                var success = await repo.ReduceStockAsync(
                    item.ProductId, item.Quantity);

                _logger.LogInformation(
                    "[STORAGE QUEUE] Stock reduced for Product {ProductId}: {Result}",
                    item.ProductId, success ? "✅" : "❌ insufficient stock");
            }
        }
    }
}