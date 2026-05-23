using Catalog.Core.Entities;
using Catalog.Core.Features.Categories.Queries.GetAllCategories;
using Catalog.Core.Interfaces;
using FluentAssertions;
using Moq;

namespace Catalog.Tests.Categories.Queries
{
    public class GetAllCategoriesQueryHandlerTests
    {
        private readonly Mock<ICategoryRepository> _repoMock = new();

        private GetAllCategoriesQueryHandler CreateHandler()
            => new(_repoMock.Object);

        [Fact]
        public async Task Handle_ShouldReturnAllCategories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new() { Id = Guid.NewGuid(), Name = "Electronics", Description = "Electronic devices" },
                new() { Id = Guid.NewGuid(), Name = "Furniture",   Description = "Home furniture"     }
            };

            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(categories);

            var query = new GetAllCategoriesQuery();

            // Act
            var result = await CreateHandler().Handle(query, CancellationToken.None);

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(c => c.Name == "Electronics");
            result.Should().Contain(c => c.Name == "Furniture");
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoCategories()
        {
            // Arrange
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Category>());

            var query = new GetAllCategoriesQuery();

            // Act
            var result = await CreateHandler().Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_ShouldCallRepositoryGetAllAsync_Once()
        {
            // Arrange
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Category>());

            // Act
            await CreateHandler().Handle(new GetAllCategoriesQuery(), CancellationToken.None);

            // Assert
            _repoMock.Verify(r => r.GetAllAsync(), Times.Once);
        }
    }
}
