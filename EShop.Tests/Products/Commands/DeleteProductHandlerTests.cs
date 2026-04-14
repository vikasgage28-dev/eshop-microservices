using EShop.Core.Features.Products.Commands;
using EShop.Core.Features.Products.Handlers;
using EShop.Core.Interfaces;
using FluentAssertions;
using Moq;

namespace EShop.Tests.Products.Commands
{
    public class DeleteProductHandlerTests
    {
        private readonly Mock<IProductRepository> _mockRepo;
        private readonly DeleteProductHandler _handler;

        public DeleteProductHandlerTests()
        {
            _mockRepo = new Mock<IProductRepository>();
            _handler = new DeleteProductHandler(_mockRepo.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnTrue_WhenProductDeleted()
        {
            // Arrange
            _mockRepo.Setup(r => r.DeleteAsync(1))
                     .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(
                new DeleteProductCommand(1),
                CancellationToken.None);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenProductNotFound()
        {
            // Arrange
            _mockRepo.Setup(r => r.DeleteAsync(99))
                     .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(
                new DeleteProductCommand(99),
                CancellationToken.None);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_ShouldCallDeleteAsync_ExactlyOnce()
        {
            // Arrange
            _mockRepo.Setup(r => r.DeleteAsync(1))
                     .ReturnsAsync(true);

            // Act
            await _handler.Handle(
                new DeleteProductCommand(1),
                CancellationToken.None);

            // Assert
            _mockRepo.Verify(r => r.DeleteAsync(1), Times.Once);
        }
    }
}