namespace Catalog.Core.Events
{
    public record ProductCreatedEvent(
        Guid    ProductId,
        string  Name,
        decimal Price,
        int     Stock,
        Guid    CategoryId,
        DateTime OccurredAt
    );
}
