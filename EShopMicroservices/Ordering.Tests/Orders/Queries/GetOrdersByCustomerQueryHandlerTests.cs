using FluentAssertions;
using Moq;
using Ordering.Core.Entities;
using Ordering.Core.Features.Orders.Queries.GetOrdersByCustomer;
using Ordering.Core.Interfaces;

namespace Ordering.Tests.Orders.Queries
{
    public class GetOrdersByCustomerQueryHandlerTests
    {
        private readonly Mock<IOrderRepository> _repoMock = new();

        private GetOrdersByCustomerQueryHandler CreateHandler()
            => new(_repoMock.Object);

        [Fact]
        public async Task Handle_ShouldReturnOrders_ForCustomer()
        {
            // Arrange
            var customerId = "customer-001";
            var query      = new GetOrdersByCustomerQuery(customerId);
            var orders     = new List<Order>
            {
                new() { Id = Guid.NewGuid(), CustomerId = customerId, CustomerEmail = "alice@eshop.com" },
                new() { Id = Guid.NewGuid(), CustomerId = customerId, CustomerEmail = "alice@eshop.com" }
            };

            _repoMock.Setup(r => r.GetByCustomerIdAsync(customerId)).ReturnsAsync(orders);

            // Act
            var result = await CreateHandler().Handle(query, CancellationToken.None);

            // Assert
            result.Should().HaveCount(2);
            result.Should().AllSatisfy(o => o.CustomerId.Should().Be(customerId));
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoOrdersForCustomer()
        {
            // Arrange
            var query = new GetOrdersByCustomerQuery("unknown-customer");
            _repoMock.Setup(r => r.GetByCustomerIdAsync(It.IsAny<string>()))
                     .ReturnsAsync(Enumerable.Empty<Order>());

            // Act
            var result = await CreateHandler().Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_ShouldCallRepository_WithCorrectCustomerId()
        {
            // Arrange
            var customerId = "customer-42";
            var query      = new GetOrdersByCustomerQuery(customerId);
            _repoMock.Setup(r => r.GetByCustomerIdAsync(customerId))
                     .ReturnsAsync(Enumerable.Empty<Order>());

            // Act
            await CreateHandler().Handle(query, CancellationToken.None);

            // Assert
            _repoMock.Verify(r => r.GetByCustomerIdAsync(customerId), Times.Once);
        }
    }
}
