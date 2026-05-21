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
        private readonly Mock<IOrderRepository>       _repoMock      = new();
        private readonly Mock<IEventPublisher>        _publisherMock = new();
        private readonly Mock<ICustomerServiceClient> _customerMock  = new();

        private static readonly Guid   CustomerId    = Guid.NewGuid();
        private static readonly string CustomerEmail = "alice@eshop.com";

        private PlaceOrderCommandHandler CreateHandler()
            => new(_repoMock.Object, _publisherMock.Object, _customerMock.Object);

        private static PlaceOrderCommand MakeCommand() => new()
        {
            CustomerId      = CustomerId.ToString(),
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
            CustomerEmail = CustomerEmail,
            Status        = OrderStatus.Pending,
            Items = cmd.Items.Select(i => new OrderItem
            {
                ProductId   = i.ProductId,
                ProductName = i.ProductName,
                UnitPrice   = i.UnitPrice,
                Quantity    = i.Quantity
            }).ToList()
        };

        private void SetupCustomerExists()
        {
            _customerMock
                .Setup(c => c.GetCustomerByIdAsync(CustomerId))
                .ReturnsAsync(new CustomerDto
                {
                    Id       = CustomerId,
                    FullName = "Alice Smith",
                    Email    = CustomerEmail
                });
        }

        [Fact]
        public async Task Handle_ShouldCreateOrderAndReturnIt()
        {
            // Arrange
            SetupCustomerExists();
            var cmd   = MakeCommand();
            var order = MakeOrder(cmd);

            _repoMock.Setup(r => r.AddAsync(It.IsAny<Order>())).ReturnsAsync(order);
            _publisherMock.Setup(p => p.PublishAsync(It.IsAny<OrderPlacedEvent>()))
                          .Returns(Task.CompletedTask);

            // Act
            var result = await CreateHandler().Handle(cmd, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.CustomerId.Should().Be(CustomerId.ToString());
            result.Status.Should().Be(OrderStatus.Pending);
        }

        [Fact]
        public async Task Handle_ShouldPublishOrderPlacedEvent_AfterSave()
        {
            // Arrange
            SetupCustomerExists();
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
            SetupCustomerExists();
            var cmd   = MakeCommand();
            var order = MakeOrder(cmd);

            _repoMock.Setup(r => r.AddAsync(It.IsAny<Order>())).ReturnsAsync(order);
            _publisherMock.Setup(p => p.PublishAsync(It.IsAny<OrderPlacedEvent>()))
                          .Returns(Task.CompletedTask);

            // Act
            await CreateHandler().Handle(cmd, CancellationToken.None);

            // Assert — repository called with correct customer info
            _repoMock.Verify(r => r.AddAsync(It.Is<Order>(o =>
                o.CustomerId    == CustomerId.ToString() &&
                o.CustomerEmail == CustomerEmail &&
                o.Items.Count   == 1)), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenCustomerNotFound()
        {
            // Arrange
            _customerMock
                .Setup(c => c.GetCustomerByIdAsync(CustomerId))
                .ReturnsAsync((CustomerDto?)null);

            var cmd = MakeCommand();

            // Act
            var act = async () => await CreateHandler().Handle(cmd, CancellationToken.None);

            // Assert — throws when customer does not exist
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"*{CustomerId}*");
        }
    }
}
