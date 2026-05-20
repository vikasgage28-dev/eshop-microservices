using Catalog.Core.Entities;
using Catalog.Core.Features.Products.Queries.GetProductById;
using Catalog.Core.Interfaces;
using FluentAssertions;
using Moq;

namespace Catalog.Tests.Products.Queries
{
    public class GetProductByIdQueryHandlerTests
    {
        private readonly Mock<IProductRepository> _repoMock = new();

        private GetProductByIdQueryHandler CreateHandler()
            => new(_repoMock.Object);

        [Fact]
        public async Task Handle_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product   = new Product { Id = productId, Name = "Laptop", Price = 999.99m };

            _repoMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(product);

            var query = new GetProductByIdQuery(productId);

            // Act
            var result = await CreateHandler().Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(productId);
            result.Name.Should().Be("Laptop");
            result.Price.Should().Be(999.99m);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenProductNotFound()
        {
            // Arrange
            var productId = Guid.NewGuid();
            _repoMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync((Product?)null);

            var query = new GetProductByIdQuery(productId);

            // Act
            var result = await CreateHandler().Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_ShouldCallRepository_WithCorrectId()
        {
            // Arrange
            var productId = Guid.NewGuid();
            _repoMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync((Product?)null);

            var query = new GetProductByIdQuery(productId);

            // Act
            await CreateHandler().Handle(query, CancellationToken.None);

            // Assert — repository called with the EXACT id from the query
            _repoMock.Verify(r => r.GetByIdAsync(productId), Times.Once);
        }
    }
}
