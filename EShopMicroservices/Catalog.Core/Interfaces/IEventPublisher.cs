namespace Catalog.Core.Interfaces
{
    /// <summary>
    /// Contract for publishing domain events.
    /// Dev  → InMemoryEventPublisher  (logs to console)
    /// Prod → ServiceBusEventPublisher (Azure Service Bus)
    /// </summary>
    public interface IEventPublisher
    {
        Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default)
            where T : class;
    }
}
