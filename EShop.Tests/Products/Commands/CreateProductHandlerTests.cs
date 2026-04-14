using EShop.Core.Features.Products.Commands;
using EShop.Core.Features.Products.Handlers;
using EShop.Core.Entities;
using EShop.Core.Interfaces;
using FluentAssertions;
using Moq;

namespace EShop.Tests.Products.Commands
{
    public class CreateProductHandlerTests
    {
        private readonly Mock<IProductRepository> _mockRepo;
        private readonly CreateProductHandler _handler;

        public CreateProductHandlerTests()
        {
            _mockRepo = new Mock<IProductRepository>();
            _handler = new CreateProductHandler(_mockRepo.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateProduct_AndReturnDto()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Name = "Smart Watch",
                Description = "Apple Watch",
                Price = 499.99m,
                Stock = 25,
                Category = "Electronics"
            };

            var createdProduct = new Product
            {
                Id = 6,
                Name = command.Name,
                Description = command.Description,
                Price = command.Price,
                Stock = command.Stock,
                Category = command.Category,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _mockRepo.Setup(r => r.CreateAsync(It.IsAny<Product>()))
                     .ReturnsAsync(createdProduct);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(6);
            result.Name.Should().Be("Smart Watch");
            result.Price.Should().Be(499.99m);
            result.Category.Should().Be("Electronics");
        }

        [Fact]
        public async Task Handle_ShouldCallCreateAsync_ExactlyOnce()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Name = "Test Product",
                Price = 99.99m,
                Stock = 10,
                Category = "Test"
            };

            _mockRepo.Setup(r => r.CreateAsync(It.IsAny<Product>()))
                     .ReturnsAsync(new Product { Id = 1, Name = "Test Product", CreatedAt = DateTime.UtcNow });

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert - verify repository was called exactly once
            _mockRepo.Verify(r => r.CreateAsync(It.IsAny<Product>()), Times.Once);
        }
    }
}