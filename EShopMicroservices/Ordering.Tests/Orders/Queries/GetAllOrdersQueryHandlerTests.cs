using FluentAssertions;
using Moq;
using Ordering.Core.Entities;
using Ordering.Core.Features.Orders.Queries.GetAllOrders;
using Ordering.Core.Interfaces;

namespace Ordering.Tests.Orders.Queries
{
    public class GetAllOrdersQueryHandlerTests
    {
        private readonly Mock<IOrderRepository> _repoMock = new();

        private GetAllOrdersQueryHandler CreateHandler()
            => new(_repoMock.Object);

        [Fact]
        public async Task Handle_ShouldReturnAllOrders()
        {
            // Arrange
            var orders = new List<Order>
            {
                new() { Id = Guid.NewGuid(), CustomerId = "customer-001", CustomerEmail = "alice@eshop.com" },
                new() { Id = Guid.NewGuid(), CustomerId = "customer-002", CustomerEmail = "bob@eshop.com" },
                new() { Id = Guid.NewGuid(), CustomerId = "customer-003", CustomerEmail = "carol@eshop.com" }
            };

            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(orders);

            // Act
            var result = await CreateHandler().Handle(new GetAllOrdersQuery(), CancellationToken.None);

            // Assert
            result.Should().HaveCount(3);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoOrders()
        {
            // Arrange
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(Enumerable.Empty<Order>());

            // Act
            var result = await CreateHandler().Handle(new GetAllOrdersQuery(), CancellationToken.None);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_ShouldCallGetAllAsync_Once()
        {
            // Arrange
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(Enumerable.Empty<Order>());

            // Act
            await CreateHandler().Handle(new GetAllOrdersQuery(), CancellationToken.None);

            // Assert
            _repoMock.Verify(r => r.GetAllAsync(), Times.Once);
        }
    }
}
