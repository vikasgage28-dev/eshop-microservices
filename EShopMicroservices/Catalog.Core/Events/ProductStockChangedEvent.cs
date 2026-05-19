namespace Catalog.Core.Events
{
    public record ProductStockChangedEvent(
        Guid     ProductId,
        int      OldStock,
        int      NewStock,
        DateTime OccurredAt
    );
}
