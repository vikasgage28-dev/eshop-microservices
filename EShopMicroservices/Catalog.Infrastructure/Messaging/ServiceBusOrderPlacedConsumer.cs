using Azure.Messaging.ServiceBus;
using Catalog.Core.Interfaces;
using EShop.Contracts.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Catalog.Infrastructure.Messaging
{
    /// <summary>
    /// SERVICE BUS CONSUMER — Learning reference.
    /// Listens to Azure Service Bus Topic Subscription.
    /// Switch via appsettings.json: "Messaging:Provider": "ServiceBus"
    /// Requires Standard tier — Topics + Subscriptions!
    /// </summary>
    public class ServiceBusOrderPlacedConsumer : IOrderPlacedConsumer
    {
        private readonly ServiceBusClient _client;
        private readonly string _topicName;
        private readonly string _subscriptionName;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ServiceBusOrderPlacedConsumer> _logger;

        public ServiceBusOrderPlacedConsumer(
            ServiceBusClient client,
            string topicName,
            string subscriptionName,
            IServiceScopeFactory scopeFactory,
            ILogger<ServiceBusOrderPlacedConsumer> logger)
        {
            _client = client;
            _topicName = topicName;
            _subscriptionName = subscriptionName;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task ConsumeAsync(CancellationToken cancellationToken)
        {
            var processor = _client.CreateProcessor(
                _topicName, _subscriptionName);

            processor.ProcessMessageAsync += async args =>
            {
                var body = args.Message.Body.ToString();
                var event_ = JsonSerializer.Deserialize<OrderPlacedEvent>(body);
                if (event_ == null) return;

                _logger.LogInformation(
                    "[SERVICE BUS] Received OrderPlaced for Order {OrderId}",
                    event_.OrderId);

                await ReduceStockAsync(event_);
                await args.CompleteMessageAsync(args.Message); // mark as done!
            };

            processor.ProcessErrorAsync += args =>
            {
                _logger.LogError(args.Exception,
                    "[SERVICE BUS] Error processing message");
                return Task.CompletedTask;
            };

            await processor.StartProcessingAsync(cancellationToken);
            await Task.Delay(Timeout.Infinite, cancellationToken);
            await processor.StopProcessingAsync();
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
                    "[SERVICE BUS] Stock reduced for Product {ProductId}: {Result}",
                    item.ProductId, success ? "✅" : "❌ insufficient stock");
            }
        }
    }
}