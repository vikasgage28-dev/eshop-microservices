using Customer.Core.Events;
using Customer.Core.Features.Customers.Commands.DeleteCustomer;
using Customer.Core.Interfaces;
using FluentAssertions;
using Moq;

namespace Customer.Tests.Customers.Commands
{
    public class DeleteCustomerCommandHandlerTests
    {
        private readonly Mock<ICustomerRepository> _repoMock      = new();
        private readonly Mock<IEventPublisher>     _publisherMock = new();

        private DeleteCustomerCommandHandler CreateHandler()
            => new(_repoMock.Object, _publisherMock.Object);

        private static Core.Entities.Customer MakeCustomer(Guid id) => new()
        {
            Id    = id,
            Email = "alice@eshop.com"
        };

        [Fact]
        public async Task Handle_ShouldReturnTrue_WhenCustomerDeleted()
        {
            // Arrange
            var id  = Guid.NewGuid();
            var cmd = new DeleteCustomerCommand { Id = id };

            _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(MakeCustomer(id));
            _repoMock.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);
            _publisherMock.Setup(p => p.PublishAsync(It.IsAny<CustomerDeletedEvent>())).Returns(Task.CompletedTask);

            // Act
            var result = await CreateHandler().Handle(cmd, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ShouldPublishCustomerDeletedEvent_WhenDeleted()
        {
            // Arrange
            var id  = Guid.NewGuid();
            var cmd = new DeleteCustomerCommand { Id = id };

            _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(MakeCustomer(id));
            _repoMock.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);
            _publisherMock.Setup(p => p.PublishAsync(It.IsAny<CustomerDeletedEvent>())).Returns(Task.CompletedTask);

            // Act
            await CreateHandler().Handle(cmd, CancellationToken.None);

            // Assert
            _publisherMock.Verify(p => p.PublishAsync(It.IsAny<CustomerDeletedEvent>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenCustomerNotFound()
        {
            // Arrange
            var cmd = new DeleteCustomerCommand { Id = Guid.NewGuid() };
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Core.Entities.Customer?)null);

            // Act
            var result = await CreateHandler().Handle(cmd, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_ShouldNotPublishEvent_WhenCustomerNotFound()
        {
            // Arrange
            var cmd = new DeleteCustomerCommand { Id = Guid.NewGuid() };
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Core.Entities.Customer?)null);

            // Act
            await CreateHandler().Handle(cmd, CancellationToken.None);

            // Assert
            _publisherMock.Verify(p => p.PublishAsync(It.IsAny<CustomerDeletedEvent>()), Times.Never);
        }
    }
}
