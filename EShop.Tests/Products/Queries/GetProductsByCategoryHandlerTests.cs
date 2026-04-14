using EShop.Core.Features.Products.Handlers;
using EShop.Core.Features.Products.Queries;
using EShop.Core.Entities;
using EShop.Core.Interfaces;
using FluentAssertions;
using Moq;

namespace EShop.Tests.Products.Queries
{
    public class GetProductsByCategoryHandlerTests
    {
        private readonly Mock<IProductRepository> _mockRepo;
        private readonly GetProductsByCategoryHandler _handler;

        public GetProductsByCategoryHandlerTests()
        {
            _mockRepo = new Mock<IProductRepository>();
            _handler = new GetProductsByCategoryHandler(_mockRepo.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnProducts_ForGivenCategory()
        {
            // Arrange
            var fakeProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Price = 999.99m, Stock = 10, Category = "Electronics", IsActive = true, CreatedAt = DateTime.UtcNow },
                new Product { Id = 2, Name = "Phone", Price = 699.99m, Stock = 20, Category = "Electronics", IsActive = true, CreatedAt = DateTime.UtcNow }
            };

            _mockRepo.Setup(r => r.GetByCategoryAsync("Electronics"))
                     .ReturnsAsync(fakeProducts);

            // Act
            var result = await _handler.Handle(
                new GetProductsByCategoryQuery("Electronics"),
                CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.All(p => p.Category == "Electronics").Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ShouldReturnEmpty_WhenNoCategoryMatch()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetByCategoryAsync("Furniture"))
                     .ReturnsAsync(new List<Product>());

            // Act
            var result = await _handler.Handle(
                new GetProductsByCategoryQuery("Furniture"),
                CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}