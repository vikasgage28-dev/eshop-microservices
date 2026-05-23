namespace Catalog.Core.Events
{
    public record ProductUpdatedEvent(
        Guid    ProductId,
        string  Name,
        decimal Price,
        int     Stock,
        Guid    CategoryId,
        bool    IsActive,
        DateTime OccurredAt
    );
}
