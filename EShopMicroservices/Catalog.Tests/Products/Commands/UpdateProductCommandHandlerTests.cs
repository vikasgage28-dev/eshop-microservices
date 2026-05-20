using Catalog.Core.Entities;
using Catalog.Core.Events;
using Catalog.Core.Features.Products.Commands.UpdateProduct;
using Catalog.Core.Interfaces;
using FluentAssertions;
using Moq;

namespace Catalog.Tests.Products.Commands
{
    public class UpdateProductCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _repoMock     = new();
        private readonly Mock<IEventPublisher>    _publisherMock = new();

        private UpdateProductCommandHandler CreateHandler()
            => new(_repoMock.Object, _publisherMock.Object);

        [Fact]
        public async Task Handle_ShouldReturnUpdatedProduct_WhenProductExists()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var cmd       = new UpdateProductCommand(productId, "Updated Laptop", "Desc", 1099.99m, 8, Guid.NewGuid(), true);
            var updated   = new Product { Id = productId, Name = cmd.Name, Price = cmd.Price };

            _repoMock.Setup(r => r.UpdateAsync(productId, It.IsAny<Product>())).ReturnsAsync(updated);
            _publisherMock.Setup(p => p.PublishAsync(It.IsAny<ProductUpdatedEvent>(), It.IsAny<CancellationToken>()))
                          .Returns(Task.CompletedTask);

            // Act
            var result = await CreateHandler().Handle(cmd, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("Updated Laptop");
            result.Price.Should().Be(1099.99m);
        }

        [Fact]
        public async Task Handle_ShouldPublishProductUpdatedEvent_WhenProductExists()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var cmd       = new UpdateProductCommand(productId, "Desk", "Adjustable", 499.99m, 5, Guid.NewGuid(), true);
            var updated   = new Product { Id = productId, Name = cmd.Name };

            _repoMock.Setup(r => r.UpdateAsync(productId, It.IsAny<Product>())).ReturnsAsync(updated);
            _publisherMock.Setup(p => p.PublishAsync(It.IsAny<ProductUpdatedEvent>(), It.IsAny<CancellationToken>()))
                          .Returns(Task.CompletedTask);

            // Act
            await CreateHandler().Handle(cmd, CancellationToken.None);

            // Assert
            _publisherMock.Verify(
                p => p.PublishAsync(It.IsAny<ProductUpdatedEvent>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenProductNotFound()
        {
            // Arrange
            var cmd = new UpdateProductCommand(Guid.NewGuid(), "X", "X", 1m, 1, Guid.NewGuid(), true);
            _repoMock.Setup(r => r.UpdateAsync(It.IsAny<Guid>(), It.IsAny<Product>()))
                     .ReturnsAsync((Product?)null);

            // Act
            var result = await CreateHandler().Handle(cmd, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_ShouldNotPublishEvent_WhenProductNotFound()
        {
            // Arrange
            var cmd = new UpdateProductCommand(Guid.NewGuid(), "X", "X", 1m, 1, Guid.NewGuid(), true);
            _repoMock.Setup(r => r.UpdateAsync(It.IsAny<Guid>(), It.IsAny<Product>()))
                     .ReturnsAsync((Product?)null);

            // Act
            await CreateHandler().Handle(cmd, CancellationToken.None);

            // Assert — no event should be published!
            _publisherMock.Verify(
                p => p.PublishAsync(It.IsAny<ProductUpdatedEvent>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}
