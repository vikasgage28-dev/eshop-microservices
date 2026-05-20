using Customer.Core.Events;
using Customer.Core.Features.Customers.Commands.UpdateCustomer;
using Customer.Core.Interfaces;
using FluentAssertions;
using Moq;

namespace Customer.Tests.Customers.Commands
{
    public class UpdateCustomerCommandHandlerTests
    {
        private readonly Mock<ICustomerRepository> _repoMock      = new();
        private readonly Mock<IEventPublisher>     _publisherMock = new();

        private UpdateCustomerCommandHandler CreateHandler()
            => new(_repoMock.Object, _publisherMock.Object);

        private static Core.Entities.Customer MakeCustomer(Guid id) => new()
        {
            Id        = id,
            FirstName = "Alice",
            LastName  = "Smith",
            Email     = "alice@eshop.com",
            UpdatedAt = DateTime.UtcNow
        };

        [Fact]
        public async Task Handle_ShouldReturnUpdatedCustomer_WhenCustomerExists()
        {
            // Arrange
            var id  = Guid.NewGuid();
            var cmd = new UpdateCustomerCommand { Id = id, FirstName = "Alice", LastName = "Updated", Email = "alice@eshop.com" };

            _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(MakeCustomer(id));
            _repoMock.Setup(r => r.UpdateAsync(It.IsAny<Core.Entities.Customer>())).ReturnsAsync(MakeCustomer(id));
            _publisherMock.Setup(p => p.PublishAsync(It.IsAny<CustomerUpdatedEvent>())).Returns(Task.CompletedTask);

            // Act
            var result = await CreateHandler().Handle(cmd, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenCustomerNotFound()
        {
            // Arrange
            var cmd = new UpdateCustomerCommand { Id = Guid.NewGuid(), FirstName = "X", LastName = "Y", Email = "x@y.com" };
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Core.Entities.Customer?)null);

            // Act
            var result = await CreateHandler().Handle(cmd, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_ShouldPublishCustomerUpdatedEvent_WhenUpdated()
        {
            // Arrange
            var id  = Guid.NewGuid();
            var cmd = new UpdateCustomerCommand { Id = id, FirstName = "Alice", LastName = "Smith", Email = "alice@eshop.com" };

            _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(MakeCustomer(id));
            _repoMock.Setup(r => r.UpdateAsync(It.IsAny<Core.Entities.Customer>())).ReturnsAsync(MakeCustomer(id));
            _publisherMock.Setup(p => p.PublishAsync(It.IsAny<CustomerUpdatedEvent>())).Returns(Task.CompletedTask);

            // Act
            await CreateHandler().Handle(cmd, CancellationToken.None);

            // Assert
            _publisherMock.Verify(p => p.PublishAsync(It.IsAny<CustomerUpdatedEvent>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldNotPublishEvent_WhenCustomerNotFound()
        {
            // Arrange
            var cmd = new UpdateCustomerCommand { Id = Guid.NewGuid(), FirstName = "X", LastName = "Y", Email = "x@y.com" };
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Core.Entities.Customer?)null);

            // Act
            await CreateHandler().Handle(cmd, CancellationToken.None);

            // Assert
            _publisherMock.Verify(p => p.PublishAsync(It.IsAny<CustomerUpdatedEvent>()), Times.Never);
        }
    }
}
