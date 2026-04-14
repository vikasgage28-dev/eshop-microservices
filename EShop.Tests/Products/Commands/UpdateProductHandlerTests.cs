using EShop.Core.Features.Products.Commands;
using EShop.Core.Features.Products.Handlers;
using EShop.Core.Entities;
using EShop.Core.Interfaces;
using FluentAssertions;
using Moq;

namespace EShop.Tests.Products.Commands
{
    public class UpdateProductHandlerTests
    {
        private readonly Mock<IProductRepository> _mockRepo;
        private readonly UpdateProductHandler _handler;

        public UpdateProductHandlerTests()
        {
            _mockRepo = new Mock<IProductRepository>();
            _handler = new UpdateProductHandler(_mockRepo.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnUpdatedProduct_WhenProductExists()
        {
            // Arrange
            var command = new UpdateProductCommand
            {
                Id = 1,
                Name = "Laptop Pro",
                Description = "Updated Laptop",
                Price = 1299.99m,
                Stock = 5,
                Category = "Electronics",
                IsActive = true
            };

            var updatedProduct = new Product
            {
                Id = 1,
                Name = command.Name,
                Description = command.Description,
                Price = command.Price,
                Stock = command.Stock,
                Category = command.Category,
                IsActive = command.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            _mockRepo.Setup(r => r.UpdateAsync(1, It.IsAny<Product>()))
                     .ReturnsAsync(updatedProduct);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(1);
            result.Name.Should().Be("Laptop Pro");
            result.Price.Should().Be(1299.99m);
            result.Stock.Should().Be(5);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenProductNotFound()
        {
            // Arrange
            var command = new UpdateProductCommand
            {
                Id = 99,
                Name = "Ghost Product",
                Price = 99.99m,
                Stock = 1,
                Category = "Unknown"
            };

            _mockRepo.Setup(r => r.UpdateAsync(99, It.IsAny<Product>()))
                     .ReturnsAsync((Product?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_ShouldCallUpdateAsync_ExactlyOnce()
        {
            // Arrange
            var command = new UpdateProductCommand
            {
                Id = 1,
                Name = "Laptop Pro",
                Price = 1299.99m,
                Stock = 5,
                Category = "Electronics"
            };

            _mockRepo.Setup(r => r.UpdateAsync(1, It.IsAny<Product>()))
                     .ReturnsAsync(new Product
                     {
                         Id = 1,
                         Name = "Laptop Pro",
                         CreatedAt = DateTime.UtcNow
                     });

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert - verify repository was called exactly once
            _mockRepo.Verify(r => r.UpdateAsync(1, It.IsAny<Product>()), Times.Once);
        }
    }
}