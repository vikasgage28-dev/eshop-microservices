using Customer.Core.Features.Customers.Queries.GetAllCustomers;
using Customer.Core.Interfaces;
using FluentAssertions;
using Moq;

namespace Customer.Tests.Customers.Queries
{
    public class GetAllCustomersQueryHandlerTests
    {
        private readonly Mock<ICustomerRepository> _repoMock = new();

        private GetAllCustomersQueryHandler CreateHandler()
            => new(_repoMock.Object);

        [Fact]
        public async Task Handle_ShouldReturnAllCustomers()
        {
            // Arrange
            var customers = new List<Core.Entities.Customer>
            {
                new() { Id = Guid.NewGuid(), FirstName = "Alice", LastName = "Smith", Email = "alice@eshop.com" },
                new() { Id = Guid.NewGuid(), FirstName = "Bob",   LastName = "Jones", Email = "bob@eshop.com"   },
                new() { Id = Guid.NewGuid(), FirstName = "Carol", LastName = "Doe",   Email = "carol@eshop.com"  }
            };

            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(customers);

            // Act
            var result = await CreateHandler().Handle(new GetAllCustomersQuery(), CancellationToken.None);

            // Assert
            result.Should().HaveCount(3);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoCustomers()
        {
            // Arrange
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(Enumerable.Empty<Core.Entities.Customer>());

            // Act
            var result = await CreateHandler().Handle(new GetAllCustomersQuery(), CancellationToken.None);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_ShouldCallGetAllAsync_Once()
        {
            // Arrange
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(Enumerable.Empty<Core.Entities.Customer>());

            // Act
            await CreateHandler().Handle(new GetAllCustomersQuery(), CancellationToken.None);

            // Assert
            _repoMock.Verify(r => r.GetAllAsync(), Times.Once);
        }
    }
}
