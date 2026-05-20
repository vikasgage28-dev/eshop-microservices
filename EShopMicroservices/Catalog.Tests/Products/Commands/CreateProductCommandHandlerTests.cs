using Catalog.Core.Entities;
using Catalog.Core.Events;
using Catalog.Core.Features.Products.Commands.CreateProduct;
using Catalog.Core.Interfaces;
using FluentAssertions;
using Moq;

namespace Catalog.Tests.Products.Commands
{
    public class CreateProductCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _repoMock     = new();
        private readonly Mock<IEventPublisher>    _publisherMock = new();

        private CreateProductCommandHandler CreateHandler()
            => new(_repoMock.Object, _publisherMock.Object);

        private static Product MakeProduct(CreateProductCommand cmd)
            => new()
            {
                Id          = Guid.NewGuid(),
                Name        = cmd.Name,
                Description = cmd.Description,
                Price       = cmd.Price,
                Stock       = cmd.Stock,
                CategoryId  = cmd.CategoryId
            };

        [Fact]
        public async Task Handle_ShouldCreateProductAndReturnIt()
        {
            // Arrange
            var cmd     = new CreateProductCommand("Laptop", "High-end", 999.99m, 10, Guid.NewGuid());
            var product = MakeProduct(cmd);

            _repoMock.Setup(r => r.CreateAsync(It.IsAny<Product>())).ReturnsAsync(product);
            _publisherMock.Setup(p => p.PublishAsync(It.IsAny<ProductCreatedEvent>(), It.IsAny<CancellationToken>()))
                          .Returns(Task.CompletedTask);

            // Act
            var result = await CreateHandler().Handle(cmd, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("Laptop");
            result.Price.Should().Be(999.99m);
        }

        [Fact]
        public async Task Handle_ShouldPublishProductCreatedEvent_AfterSave()
        {
            // Arrange
            var cmd     = new CreateProductCommand("Mouse", "Wireless", 29.99m, 50, Guid.NewGuid());
            var product = MakeProduct(cmd);

            _repoMock.Setup(r => r.CreateAsync(It.IsAny<Product>())).ReturnsAsync(product);
            _publisherMock.Setup(p => p.PublishAsync(It.IsAny<ProductCreatedEvent>(), It.IsAny<CancellationToken>()))
                          .Returns(Task.CompletedTask);

            // Act
            await CreateHandler().Handle(cmd, CancellationToken.None);

            // Assert — event published exactly once!
            _publisherMock.Verify(
                p => p.PublishAsync(It.IsAny<ProductCreatedEvent>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldCallRepository_WithCorrectData()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var cmd        = new CreateProductCommand("Keyboard", "Mechanical", 149.99m, 20, categoryId);
            var product    = MakeProduct(cmd);

            _repoMock.Setup(r => r.CreateAsync(It.IsAny<Product>())).ReturnsAsync(product);
            _publisherMock.Setup(p => p.PublishAsync(It.IsAny<ProductCreatedEvent>(), It.IsAny<CancellationToken>()))
                          .Returns(Task.CompletedTask);

            // Act
            await CreateHandler().Handle(cmd, CancellationToken.None);

            // Assert — repository called with matching data
            _repoMock.Verify(r => r.CreateAsync(It.Is<Product>(p =>
                p.Name       == "Keyboard" &&
                p.Price      == 149.99m    &&
                p.CategoryId == categoryId)), Times.Once);
        }
    }
}
