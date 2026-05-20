using Catalog.Core.Events;
using Catalog.Core.Features.Products.Commands.DeleteProduct;
using Catalog.Core.Interfaces;
using FluentAssertions;
using Moq;

namespace Catalog.Tests.Products.Commands
{
    public class DeleteProductCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _repoMock     = new();
        private readonly Mock<IEventPublisher>    _publisherMock = new();

        private DeleteProductCommandHandler CreateHandler()
            => new(_repoMock.Object, _publisherMock.Object);

        [Fact]
        public async Task Handle_ShouldReturnTrue_WhenProductDeleted()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var cmd       = new DeleteProductCommand(productId);

            _repoMock.Setup(r => r.DeleteAsync(productId)).ReturnsAsync(true);
            _publisherMock.Setup(p => p.PublishAsync(It.IsAny<ProductDeletedEvent>(), It.IsAny<CancellationToken>()))
                          .Returns(Task.CompletedTask);

            // Act
            var result = await CreateHandler().Handle(cmd, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ShouldPublishProductDeletedEvent_WhenDeleted()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var cmd       = new DeleteProductCommand(productId);

            _repoMock.Setup(r => r.DeleteAsync(productId)).ReturnsAsync(true);
            _publisherMock.Setup(p => p.PublishAsync(It.IsAny<ProductDeletedEvent>(), It.IsAny<CancellationToken>()))
                          .Returns(Task.CompletedTask);

            // Act
            await CreateHandler().Handle(cmd, CancellationToken.None);

            // Assert
            _publisherMock.Verify(
                p => p.PublishAsync(It.IsAny<ProductDeletedEvent>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenProductNotFound()
        {
            // Arrange
            var cmd = new DeleteProductCommand(Guid.NewGuid());
            _repoMock.Setup(r => r.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(false);

            // Act
            var result = await CreateHandler().Handle(cmd, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_ShouldNotPublishEvent_WhenProductNotFound()
        {
            // Arrange
            var cmd = new DeleteProductCommand(Guid.NewGuid());
            _repoMock.Setup(r => r.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(false);

            // Act
            await CreateHandler().Handle(cmd, CancellationToken.None);

            // Assert — no event when product didn't exist!
            _publisherMock.Verify(
                p => p.PublishAsync(It.IsAny<ProductDeletedEvent>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}
