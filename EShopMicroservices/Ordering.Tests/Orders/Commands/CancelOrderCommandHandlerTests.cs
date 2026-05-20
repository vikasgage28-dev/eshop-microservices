using FluentAssertions;
using Moq;
using Ordering.Core.Entities;
using Ordering.Core.Events;
using Ordering.Core.Features.Orders.Commands.CancelOrder;
using Ordering.Core.Interfaces;

namespace Ordering.Tests.Orders.Commands
{
    public class CancelOrderCommandHandlerTests
    {
        private readonly Mock<IOrderRepository> _repoMock      = new();
        private readonly Mock<IEventPublisher>  _publisherMock = new();

        private CancelOrderCommandHandler CreateHandler()
            => new(_repoMock.Object, _publisherMock.Object);

        private static Order MakeOrder(Guid orderId) => new()
        {
            Id            = orderId,
            CustomerId    = "customer-001",
            CustomerEmail = "alice@eshop.com",
            Status        = OrderStatus.Pending
        };

        [Fact]
        public async Task Handle_ShouldReturnTrue_WhenOrderCancelled()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var cmd     = new CancelOrderCommand { OrderId = orderId, Reason = "Changed my mind" };
            var order   = MakeOrder(orderId);

            _repoMock.Setup(r => r.GetByIdAsync(orderId)).ReturnsAsync(order);
            _repoMock.Setup(r => r.UpdateAsync(It.IsAny<Order>())).ReturnsAsync(order);
            _publisherMock.Setup(p => p.PublishAsync(It.IsAny<OrderCancelledEvent>()))
                          .Returns(Task.CompletedTask);

            // Act
            var result = await CreateHandler().Handle(cmd, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ShouldPublishOrderCancelledEvent_WhenCancelled()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var cmd     = new CancelOrderCommand { OrderId = orderId, Reason = "Out of stock" };
            var order   = MakeOrder(orderId);

            _repoMock.Setup(r => r.GetByIdAsync(orderId)).ReturnsAsync(order);
            _repoMock.Setup(r => r.UpdateAsync(It.IsAny<Order>())).ReturnsAsync(order);
            _publisherMock.Setup(p => p.PublishAsync(It.IsAny<OrderCancelledEvent>()))
                          .Returns(Task.CompletedTask);

            // Act
            await CreateHandler().Handle(cmd, CancellationToken.None);

            // Assert — event published exactly once!
            _publisherMock.Verify(
                p => p.PublishAsync(It.IsAny<OrderCancelledEvent>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenOrderNotFound()
        {
            // Arrange
            var cmd = new CancelOrderCommand { OrderId = Guid.NewGuid(), Reason = "Test" };
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Order?)null);

            // Act
            var result = await CreateHandler().Handle(cmd, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_ShouldNotPublishEvent_WhenOrderNotFound()
        {
            // Arrange
            var cmd = new CancelOrderCommand { OrderId = Guid.NewGuid(), Reason = "Test" };
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Order?)null);

            // Act
            await CreateHandler().Handle(cmd, CancellationToken.None);

            // Assert — no event when order didn't exist!
            _publisherMock.Verify(
                p => p.PublishAsync(It.IsAny<OrderCancelledEvent>()),
                Times.Never);
        }
    }
}
