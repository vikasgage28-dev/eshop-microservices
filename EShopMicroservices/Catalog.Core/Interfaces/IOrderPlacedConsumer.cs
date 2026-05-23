namespace Catalog.Core.Interfaces
{
    public interface IOrderPlacedConsumer
    {
        Task ConsumeAsync(CancellationToken cancellationToken);
    }
}