using Customer.Core.Events;
using Customer.Core.Features.Customers.Commands.CreateCustomer;
using Customer.Core.Interfaces;
using FluentAssertions;
using Moq;

namespace Customer.Tests.Customers.Commands
{
    public class CreateCustomerCommandHandlerTests
    {
        private readonly Mock<ICustomerRepository> _repoMock      = new();
        private readonly Mock<IEventPublisher>     _publisherMock = new();

        private CreateCustomerCommandHandler CreateHandler()
            => new(_repoMock.Object, _publisherMock.Object);

        private static Core.Entities.Customer MakeCustomer() => new()
        {
            Id        = Guid.NewGuid(),
            FirstName = "Alice",
            LastName  = "Smith",
            Email     = "alice@eshop.com",
            Phone     = "+91-9000000001",
            CreatedAt = DateTime.UtcNow
        };

        [Fact]
        public async Task Handle_ShouldCreateCustomerAndReturnIt()
        {
            // Arrange
            var cmd      = new CreateCustomerCommand { FirstName = "Alice", LastName = "Smith", Email = "alice@eshop.com" };
            var customer = MakeCustomer();

            _repoMock.Setup(r => r.AddAsync(It.IsAny<Core.Entities.Customer>())).ReturnsAsync(customer);
            _publisherMock.Setup(p => p.PublishAsync(It.IsAny<CustomerCreatedEvent>())).Returns(Task.CompletedTask);

            // Act
            var result = await CreateHandler().Handle(cmd, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().Be("alice@eshop.com");
        }

        [Fact]
        public async Task Handle_ShouldCallRepository_WithCorrectData()
        {
            // Arrange
            var cmd      = new CreateCustomerCommand { FirstName = "Bob", LastName = "Jones", Email = "bob@eshop.com", Phone = "+91-9000000002" };
            var customer = MakeCustomer();

            _repoMock.Setup(r => r.AddAsync(It.IsAny<Core.Entities.Customer>())).ReturnsAsync(customer);
            _publisherMock.Setup(p => p.PublishAsync(It.IsAny<CustomerCreatedEvent>())).Returns(Task.CompletedTask);

            // Act
            await CreateHandler().Handle(cmd, CancellationToken.None);

            // Assert
            _repoMock.Verify(r => r.AddAsync(It.Is<Core.Entities.Customer>(c =>
                c.FirstName == "Bob" &&
                c.LastName  == "Jones" &&
                c.Email     == "bob@eshop.com")), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldPublishCustomerCreatedEvent_AfterSave()
        {
            // Arrange
            var cmd      = new CreateCustomerCommand { FirstName = "Carol", LastName = "Doe", Email = "carol@eshop.com" };
            var customer = MakeCustomer();

            _repoMock.Setup(r => r.AddAsync(It.IsAny<Core.Entities.Customer>())).ReturnsAsync(customer);
            _publisherMock.Setup(p => p.PublishAsync(It.IsAny<CustomerCreatedEvent>())).Returns(Task.CompletedTask);

            // Act
            await CreateHandler().Handle(cmd, CancellationToken.None);

            // Assert
            _publisherMock.Verify(p => p.PublishAsync(It.IsAny<CustomerCreatedEvent>()), Times.Once);
        }
    }
}
