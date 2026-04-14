using EShop.Core.Features.Products.Handlers;
using EShop.Core.Features.Products.Queries;
using EShop.Core.Entities;
using EShop.Core.Interfaces;
using FluentAssertions;
using Moq;

namespace EShop.Tests.Products.Queries
{
    public class GetProductByIdHandlerTests
    {
        private readonly Mock<IProductRepository> _mockRepo;
        private readonly GetProductByIdHandler _handler;

        public GetProductByIdHandlerTests()
        {
            _mockRepo = new Mock<IProductRepository>();
            _handler = new GetProductByIdHandler(_mockRepo.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var fakeProduct = new Product
            {
                Id = 1,
                Name = "Laptop",
                Price = 999.99m,
                Stock = 10,
                Category = "Electronics",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _mockRepo.Setup(r => r.GetByIdAsync(1))
                     .ReturnsAsync(fakeProduct);

            // Act
            var result = await _handler.Handle(
                new GetProductByIdQuery(1),
                CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(1);
            result.Name.Should().Be("Laptop");
            result.Price.Should().Be(999.99m);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenProductNotFound()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetByIdAsync(99))
                     .ReturnsAsync((Product?)null);

            // Act
            var result = await _handler.Handle(
                new GetProductByIdQuery(99),
                CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }
    }
}