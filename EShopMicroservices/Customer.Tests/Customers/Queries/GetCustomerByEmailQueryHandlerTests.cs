using Customer.Core.Features.Customers.Queries.GetCustomerByEmail;
using Customer.Core.Interfaces;
using FluentAssertions;
using Moq;

namespace Customer.Tests.Customers.Queries
{
    public class GetCustomerByEmailQueryHandlerTests
    {
        private readonly Mock<ICustomerRepository> _repoMock = new();

        private GetCustomerByEmailQueryHandler CreateHandler()
            => new(_repoMock.Object);

        private static Core.Entities.Customer MakeCustomer(string email) => new()
        {
            Id        = Guid.NewGuid(),
            FirstName = "Alice",
            LastName  = "Smith",
            Email     = email
        };

        [Fact]
        public async Task Handle_ShouldReturnCustomer_WhenEmailExists()
        {
            // Arrange
            var email = "alice@eshop.com";
            var query = new GetCustomerByEmailQuery(email);
            _repoMock.Setup(r => r.GetByEmailAsync(email)).ReturnsAsync(MakeCustomer(email));

            // Act
            var result = await CreateHandler().Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.Email.Should().Be(email);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenEmailNotFound()
        {
            // Arrange
            var query = new GetCustomerByEmailQuery("unknown@eshop.com");
            _repoMock.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((Core.Entities.Customer?)null);

            // Act
            var result = await CreateHandler().Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_ShouldCallRepository_WithCorrectEmail()
        {
            // Arrange
            var email = "bob@eshop.com";
            var query = new GetCustomerByEmailQuery(email);
            _repoMock.Setup(r => r.GetByEmailAsync(email)).ReturnsAsync(MakeCustomer(email));

            // Act
            await CreateHandler().Handle(query, CancellationToken.None);

            // Assert
            _repoMock.Verify(r => r.GetByEmailAsync(email), Times.Once);
        }
    }
}
