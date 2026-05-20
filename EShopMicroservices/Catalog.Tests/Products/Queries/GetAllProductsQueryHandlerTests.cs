using Catalog.Core.Entities;
using Catalog.Core.Features.Products.Queries.GetAllProducts;
using Catalog.Core.Interfaces;
using FluentAssertions;
using Moq;

namespace Catalog.Tests.Products.Queries
{
    public class GetAllProductsQueryHandlerTests
    {
        private readonly Mock<IProductRepository> _repoMock = new();

        private GetAllProductsQueryHandler CreateHandler()
            => new(_repoMock.Object);

        [Fact]
        public async Task Handle_ShouldReturnPagedProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new() { Id = Guid.NewGuid(), Name = "Laptop",        Price = 999.99m },
                new() { Id = Guid.NewGuid(), Name = "Wireless Mouse", Price = 29.99m  }
            };

            _repoMock.Setup(r => r.GetPagedAsync(1, 10, null, null))
                     .ReturnsAsync((products, 2));

            var query = new GetAllProductsQuery(Page: 1, PageSize: 10);

            // Act
            var (result, totalCount) = await CreateHandler().Handle(query, CancellationToken.None);

            // Assert
            result.Should().HaveCount(2);
            totalCount.Should().Be(2);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoProducts()
        {
            // Arrange
            _repoMock.Setup(r => r.GetPagedAsync(It.IsAny<int>(), It.IsAny<int>(), null, null))
                     .ReturnsAsync((Enumerable.Empty<Product>(), 0));

            var query = new GetAllProductsQuery();

            // Act
            var (result, totalCount) = await CreateHandler().Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEmpty();
            totalCount.Should().Be(0);
        }

        [Fact]
        public async Task Handle_ShouldPassSearchAndCategoryId_ToRepository()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var query      = new GetAllProductsQuery(Page: 2, PageSize: 5, Search: "laptop", CategoryId: categoryId);

            _repoMock.Setup(r => r.GetPagedAsync(2, 5, "laptop", categoryId))
                     .ReturnsAsync((Enumerable.Empty<Product>(), 0));

            // Act
            await CreateHandler().Handle(query, CancellationToken.None);

            // Assert — repository was called with the correct parameters!
            _repoMock.Verify(r => r.GetPagedAsync(2, 5, "laptop", categoryId), Times.Once);
        }
    }
}
