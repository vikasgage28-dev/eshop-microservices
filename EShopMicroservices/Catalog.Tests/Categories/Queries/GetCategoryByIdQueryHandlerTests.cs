using Catalog.Core.Entities;
using Catalog.Core.Features.Categories.Queries.GetCategoryById;
using Catalog.Core.Interfaces;
using FluentAssertions;
using Moq;

namespace Catalog.Tests.Categories.Queries
{
    public class GetCategoryByIdQueryHandlerTests
    {
        private readonly Mock<ICategoryRepository> _repoMock = new();

        private GetCategoryByIdQueryHandler CreateHandler()
            => new(_repoMock.Object);

        [Fact]
        public async Task Handle_ShouldReturnCategory_WhenCategoryExists()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var category   = new Category { Id = categoryId, Name = "Electronics", Description = "Electronic devices" };

            _repoMock.Setup(r => r.GetByIdAsync(categoryId)).ReturnsAsync(category);

            var query = new GetCategoryByIdQuery(categoryId);

            // Act
            var result = await CreateHandler().Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(categoryId);
            result.Name.Should().Be("Electronics");
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenCategoryNotFound()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            _repoMock.Setup(r => r.GetByIdAsync(categoryId)).ReturnsAsync((Category?)null);

            var query = new GetCategoryByIdQuery(categoryId);

            // Act
            var result = await CreateHandler().Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_ShouldCallRepository_WithCorrectId()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            _repoMock.Setup(r => r.GetByIdAsync(categoryId)).ReturnsAsync((Category?)null);

            var query = new GetCategoryByIdQuery(categoryId);

            // Act
            await CreateHandler().Handle(query, CancellationToken.None);

            // Assert — repository called with the exact id from the query
            _repoMock.Verify(r => r.GetByIdAsync(categoryId), Times.Once);
        }
    }
}
