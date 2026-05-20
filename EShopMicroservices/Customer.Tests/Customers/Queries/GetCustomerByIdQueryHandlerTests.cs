using Customer.Core.Features.Customers.Queries.GetCustomerById;
using Customer.Core.Interfaces;
using FluentAssertions;
using Moq;

namespace Customer.Tests.Customers.Queries
{
    public class GetCustomerByIdQueryHandlerTests
    {
        private readonly Mock<ICustomerRepository> _repoMock = new();

        private GetCustomerByIdQueryHandler CreateHandler()
            => new(_repoMock.Object);

        private static Core.Entities.Customer MakeCustomer(Guid id) => new()
        {
            Id        = id,
            FirstName = "Alice",
            LastName  = "Smith",
            Email     = "alice@eshop.com"
        };

        [Fact]
        public async Task Handle_ShouldReturnCustomer_WhenCustomerExists()
        {
            // Arrange
            var id    = Guid.NewGuid();
            var query = new GetCustomerByIdQuery(id);
            _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(MakeCustomer(id));

            // Act
            var result = await CreateHandler().Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(id);
            result.FirstName.Should().Be("Alice");
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenCustomerNotFound()
        {
            // Arrange
            var query = new GetCustomerByIdQuery(Guid.NewGuid());
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Core.Entities.Customer?)null);

            // Act
            var result = await CreateHandler().Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_ShouldCallRepository_WithCorrectId()
        {
            // Arrange
            var id    = Guid.NewGuid();
            var query = new GetCustomerByIdQuery(id);
            _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(MakeCustomer(id));

            // Act
            await CreateHandler().Handle(query, CancellationToken.None);

            // Assert
            _repoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        }
    }
}
