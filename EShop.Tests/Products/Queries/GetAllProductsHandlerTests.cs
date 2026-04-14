using EShop.Core.Features.Products.Handlers;
using EShop.Core.Features.Products.Queries;
using EShop.Core.Entities;
using EShop.Core.Interfaces;
using FluentAssertions;
using Moq;

namespace EShop.Tests.Products.Queries
{
    public class GetAllProductsHandlerTests
    {
        private readonly Mock<IProductRepository> _mockRepo;
        private readonly GetAllProductsHandler _handler;

        public GetAllProductsHandlerTests()
        {
            _mockRepo = new Mock<IProductRepository>();
            _handler = new GetAllProductsHandler(_mockRepo.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnAllProducts_WhenProductsExist()
        {
            // Arrange
            var fakeProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Price = 999.99m, Stock = 10, Category = "Electronics", IsActive = true, CreatedAt = DateTime.UtcNow },
                new Product { Id = 2, Name = "Mouse", Price = 29.99m, Stock = 50, Category = "Electronics", IsActive = true, CreatedAt = DateTime.UtcNow }
            };

            _mockRepo.Setup(r => r.GetAllAsync())
                     .ReturnsAsync(fakeProducts);

            // Act
            var result = await _handler.Handle(
                new GetAllProductsQuery(),
                CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.First().Name.Should().Be("Laptop");
            result.Last().Name.Should().Be("Mouse");
        }

        [Fact]
        public async Task Handle_ShouldReturnEmpty_WhenNoProductsExist()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetAllAsync())
                     .ReturnsAsync(new List<Product>());

            // Act
            var result = await _handler.Handle(
                new GetAllProductsQuery(),
                CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_ShouldMapProductToDto_Correctly()
        {
            // Arrange
            var fakeProduct = new Product
            {
                Id = 1,
                Name = "Laptop",
                Description = "Gaming Laptop",
                Price = 999.99m,
                Stock = 10,
                Category = "Electronics",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _mockRepo.Setup(r => r.GetAllAsync())
                     .ReturnsAsync(new List<Product> { fakeProduct });

            // Act
            var result = await _handler.Handle(
                new GetAllProductsQuery(),
                CancellationToken.None);

            // Assert
            var dto = result.First();
            dto.Id.Should().Be(1);
            dto.Name.Should().Be("Laptop");
            dto.Description.Should().Be("Gaming Laptop");
            dto.Price.Should().Be(999.99m);
            dto.Stock.Should().Be(10);
            dto.Category.Should().Be("Electronics");
        }
    }
}