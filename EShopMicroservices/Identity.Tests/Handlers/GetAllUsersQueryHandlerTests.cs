using FluentAssertions;
using Identity.Core.Entities;
using Identity.Core.Features.Auth.Queries.GetAllUsers;
using Identity.Core.Interfaces;
using Moq;

namespace Identity.Tests.Handlers;

public class GetAllUsersQueryHandlerTests
{
    private readonly Mock<IAuthRepository> _authRepo = new();

    private GetAllUsersQueryHandler CreateHandler() =>
        new(_authRepo.Object);

    [Fact]
    public async Task Handle_ReturnsAllUsers()
    {
        // Arrange
        var users = new List<ApplicationUser>
        {
            new() { Id = "1", FirstName = "Alice", LastName = "Smith",  Email = "alice@example.com" },
            new() { Id = "2", FirstName = "Bob",   LastName = "Jones",  Email = "bob@example.com"   },
            new() { Id = "3", FirstName = "Carol",  LastName = "White", Email = "carol@example.com" }
        };
        _authRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

        // Act
        var result = (await CreateHandler().Handle(new GetAllUsersQuery(), CancellationToken.None)).ToList();

        // Assert
        result.Should().HaveCount(3);
        result.Select(u => u.Email).Should().BeEquivalentTo(
            new[] { "alice@example.com", "bob@example.com", "carol@example.com" });
    }

    [Fact]
    public async Task Handle_EmptyRepository_ReturnsEmptyList()
    {
        // Arrange
        _authRepo.Setup(r => r.GetAllAsync())
                 .ReturnsAsync(new List<ApplicationUser>());

        // Act
        var result = await CreateHandler().Handle(new GetAllUsersQuery(), CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_CallsRepositoryOnce()
    {
        // Arrange
        _authRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<ApplicationUser>());

        // Act
        await CreateHandler().Handle(new GetAllUsersQuery(), CancellationToken.None);

        // Assert
        _authRepo.Verify(r => r.GetAllAsync(), Times.Once);
    }
}
