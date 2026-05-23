namespace Catalog.Core.Events
{
    public record ProductDeletedEvent(
        Guid     ProductId,
        DateTime OccurredAt
    );
}
