using EShop.Core.Entities;
using EShop.Core.Features.Products.Handlers;
using EShop.Core.Features.Products.Queries;
using EShop.Core.Interfaces;
using FluentAssertions;
using Moq;

namespace EShop.Tests.Products.Queries
{
    public class GetProductsHandlerTests
    {
        private readonly Mock<IProductRepository> _mockRepo;
        private readonly GetProductsHandler _handler;

        public GetProductsHandlerTests()
        {
            _mockRepo = new Mock<IProductRepository>();
            _handler = new GetProductsHandler(_mockRepo.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnPagedResult_WithCorrectMetadata()
        {
            // Arrange
            var fakeProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Price = 999.99m, Stock = 10, Category = "Electronics", IsActive = true, CreatedAt = DateTime.UtcNow },
                new Product { Id = 2, Name = "Phone", Price = 699.99m, Stock = 20, Category = "Electronics", IsActive = true, CreatedAt = DateTime.UtcNow }
            };

            _mockRepo.Setup(r => r.GetPagedAsync(1, 10, null, null))
                     .ReturnsAsync((fakeProducts, 2));

            var query = new GetProductsQuery { Page = 1, PageSize = 10 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().HaveCount(2);
            result.Page.Should().Be(1);
            result.PageSize.Should().Be(10);
            result.TotalCount.Should().Be(2);
            result.TotalPages.Should().Be(1);
            result.HasNextPage.Should().BeFalse();
            result.HasPreviousPage.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_ShouldReturnCorrectTotalPages_WhenMultiplePagesExist()
        {
            // Arrange
            // Simulating page 1 of 3 pages (30 total, pageSize 10)
            var fakeProducts = Enumerable.Range(1, 10).Select(i => new Product
            {
                Id = i,
                Name = $"Product {i}",
                Price = 99.99m,
                Stock = 10,
                Category = "Electronics",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }).ToList();

            _mockRepo.Setup(r => r.GetPagedAsync(1, 10, null, null))
                     .ReturnsAsync((fakeProducts, 30));

            var query = new GetProductsQuery { Page = 1, PageSize = 10 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.TotalCount.Should().Be(30);
            result.TotalPages.Should().Be(3);
            result.HasNextPage.Should().BeTrue();
            result.HasPreviousPage.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_ShouldReturnHasPreviousPage_WhenOnPageTwo()
        {
            // Arrange
            var fakeProducts = new List<Product>
            {
                new Product { Id = 11, Name = "Product 11", Price = 99.99m, Stock = 5, Category = "Electronics", IsActive = true, CreatedAt = DateTime.UtcNow }
            };

            _mockRepo.Setup(r => r.GetPagedAsync(2, 10, null, null))
                     .ReturnsAsync((fakeProducts, 11));

            var query = new GetProductsQuery { Page = 2, PageSize = 10 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Page.Should().Be(2);
            result.HasPreviousPage.Should().BeTrue();
            result.HasNextPage.Should().BeFalse();
            result.TotalPages.Should().Be(2);
        }

        [Fact]
        public async Task Handle_ShouldPassSearchTerm_ToRepository()
        {
            // Arrange
            var fakeProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop Pro", Price = 999.99m, Stock = 10, Category = "Electronics", IsActive = true, CreatedAt = DateTime.UtcNow }
            };

            _mockRepo.Setup(r => r.GetPagedAsync(1, 10, "laptop", null))
                     .ReturnsAsync((fakeProducts, 1));

            var query = new GetProductsQuery { Page = 1, PageSize = 10, Search = "laptop" };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Data.Should().HaveCount(1);
            result.Data.First().Name.Should().Be("Laptop Pro");

            // Verify search was passed to repository
            _mockRepo.Verify(r => r.GetPagedAsync(1, 10, "laptop", null), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldPassCategoryFilter_ToRepository()
        {
            // Arrange
            var fakeProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Price = 999.99m, Stock = 10, Category = "Electronics", IsActive = true, CreatedAt = DateTime.UtcNow },
                new Product { Id = 2, Name = "Phone", Price = 699.99m, Stock = 20, Category = "Electronics", IsActive = true, CreatedAt = DateTime.UtcNow }
            };

            _mockRepo.Setup(r => r.GetPagedAsync(1, 10, null, "Electronics"))
                     .ReturnsAsync((fakeProducts, 2));

            var query = new GetProductsQuery { Page = 1, PageSize = 10, Category = "Electronics" };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Data.Should().HaveCount(2);
            result.Data.All(p => p.Category == "Electronics").Should().BeTrue();

            // Verify category was passed to repository
            _mockRepo.Verify(r => r.GetPagedAsync(1, 10, null, "Electronics"), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldUseValidatedPageSize_WhenPageSizeExceedsMax()
        {
            // Arrange - client sends 500 but max allowed is 100
            _mockRepo.Setup(r => r.GetPagedAsync(1, 100, null, null))
                     .ReturnsAsync((new List<Product>(), 0));

            var query = new GetProductsQuery { Page = 1, PageSize = 500 };

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert - verify repo was called with 100 not 500
            _mockRepo.Verify(r => r.GetPagedAsync(1, 100, null, null), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmpty_WhenNoProductsFound()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetPagedAsync(1, 10, "xyz123", null))
                     .ReturnsAsync((new List<Product>(), 0));

            var query = new GetProductsQuery { Page = 1, PageSize = 10, Search = "xyz123" };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Data.Should().BeEmpty();
            result.TotalCount.Should().Be(0);
            result.TotalPages.Should().Be(0);
            result.HasNextPage.Should().BeFalse();
            result.HasPreviousPage.Should().BeFalse();
        }
    }
}