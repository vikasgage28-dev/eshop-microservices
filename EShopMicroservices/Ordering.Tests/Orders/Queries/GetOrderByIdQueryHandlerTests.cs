using FluentAssertions;
using Moq;
using Ordering.Core.Entities;
using Ordering.Core.Features.Orders.Queries.GetOrderById;
using Ordering.Core.Interfaces;

namespace Ordering.Tests.Orders.Queries
{
    public class GetOrderByIdQueryHandlerTests
    {
        private readonly Mock<IOrderRepository> _repoMock = new();

        private GetOrderByIdQueryHandler CreateHandler()
            => new(_repoMock.Object);

        private static Order MakeOrder(Guid id) => new()
        {
            Id            = id,
            CustomerId    = "customer-001",
            CustomerEmail = "alice@eshop.com",
            Status        = OrderStatus.Pending,
            Items         = new List<OrderItem>
            {
                new() { ProductName = "Laptop", UnitPrice = 999.99m, Quantity = 1 }
            }
        };

        [Fact]
        public async Task Handle_ShouldReturnOrder_WhenOrderExists()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var order   = MakeOrder(orderId);
            var query   = new GetOrderByIdQuery(orderId);

            _repoMock.Setup(r => r.GetByIdAsync(orderId)).ReturnsAsync(order);

            // Act
            var result = await CreateHandler().Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(orderId);
            result.CustomerId.Should().Be("customer-001");
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenOrderNotFound()
        {
            // Arrange
            var query = new GetOrderByIdQuery(Guid.NewGuid());
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Order?)null);

            // Act
            var result = await CreateHandler().Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_ShouldCallRepository_WithCorrectId()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var query   = new GetOrderByIdQuery(orderId);
            _repoMock.Setup(r => r.GetByIdAsync(orderId)).ReturnsAsync(MakeOrder(orderId));

            // Act
            await CreateHandler().Handle(query, CancellationToken.None);

            // Assert
            _repoMock.Verify(r => r.GetByIdAsync(orderId), Times.Once);
        }
    }
}
