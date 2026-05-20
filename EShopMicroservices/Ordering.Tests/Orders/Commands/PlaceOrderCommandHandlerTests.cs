using FluentAssertions;
using Moq;
using Ordering.Core.Entities;
using Ordering.Core.Events;
using Ordering.Core.Features.Orders.Commands.PlaceOrder;
using Ordering.Core.Interfaces;

namespace Ordering.Tests.Orders.Commands
{
    public class PlaceOrderCommandHandlerTests
    {
        private readonly Mock<IOrderRepository> _repoMock      = new();
        private readonly Mock<IEventPublisher>  _publisherMock = new();

        private PlaceOrderCommandHandler CreateHandler()
            => new(_repoMock.Object, _publisherMock.Object);

        private static PlaceOrderCommand MakeCommand() => new()
        {
            CustomerId    = "customer-001",
            CustomerEmail = "alice@eshop.com",
            ShippingAddress = "123 Main St",
            Items = new List<PlaceOrderItemDto>
            {
                new() { ProductId = Guid.NewGuid(), ProductName = "Laptop", UnitPrice = 999.99m, Quantity = 1 }
            }
        };

        private static Order MakeOrder(PlaceOrderCommand cmd) => new()
        {
            Id            = Guid.NewGuid(),
            CustomerId    = cmd.CustomerId,
            CustomerEmail = cmd.CustomerEmail,
            Status        = OrderStatus.Pending,
            Items = cmd.Items.Select(i => new OrderItem
            {
                ProductId   = i.ProductId,
                ProductName = i.ProductName,
                UnitPrice   = i.UnitPrice,
                Quantity    = i.Quantity
            }).ToList()
        };

        [Fact]
        public async Task Handle_ShouldCreateOrderAndReturnIt()
        {
            // Arrange
            var cmd   = MakeCommand();
            var order = MakeOrder(cmd);

            _repoMock.Setup(r => r.AddAsync(It.IsAny<Order>())).ReturnsAsync(order);
            _publisherMock.Setup(p => p.PublishAsync(It.IsAny<OrderPlacedEvent>()))
                          .Returns(Task.CompletedTask);

            // Act
            var result = await CreateHandler().Handle(cmd, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.CustomerId.Should().Be("customer-001");
            result.Status.Should().Be(OrderStatus.Pending);
        }

        [Fact]
        public async Task Handle_ShouldPublishOrderPlacedEvent_AfterSave()
        {
            // Arrange
            var cmd   = MakeCommand();
            var order = MakeOrder(cmd);

            _repoMock.Setup(r => r.AddAsync(It.IsAny<Order>())).ReturnsAsync(order);
            _publisherMock.Setup(p => p.PublishAsync(It.IsAny<OrderPlacedEvent>()))
                          .Returns(Task.CompletedTask);

            // Act
            await CreateHandler().Handle(cmd, CancellationToken.None);

            // Assert — event published exactly once!
            _publisherMock.Verify(
                p => p.PublishAsync(It.IsAny<OrderPlacedEvent>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldCallRepository_WithCorrectData()
        {
            // Arrange
            var cmd   = MakeCommand();
            var order = MakeOrder(cmd);

            _repoMock.Setup(r => r.AddAsync(It.IsAny<Order>())).ReturnsAsync(order);
            _publisherMock.Setup(p => p.PublishAsync(It.IsAny<OrderPlacedEvent>()))
                          .Returns(Task.CompletedTask);

            // Act
            await CreateHandler().Handle(cmd, CancellationToken.None);

            // Assert — repository called with correct customer info
            _repoMock.Verify(r => r.AddAsync(It.Is<Order>(o =>
                o.CustomerId    == "customer-001" &&
                o.CustomerEmail == "alice@eshop.com" &&
                o.Items.Count   == 1)), Times.Once);
        }
    }
}
