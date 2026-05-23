using FluentAssertions;
using Identity.Core.Entities;
using Identity.Core.Features.Auth.Queries.GetUserById;
using Identity.Core.Interfaces;
using Moq;

namespace Identity.Tests.Handlers;

public class GetUserByIdQueryHandlerTests
{
    private readonly Mock<IAuthRepository> _authRepo = new();

    private GetUserByIdQueryHandler CreateHandler() =>
        new(_authRepo.Object);

    [Fact]
    public async Task Handle_ExistingUserId_ReturnsUser()
    {
        // Arrange
        var user = new ApplicationUser
        {
            Id        = "user-42",
            Email     = "bob@example.com",
            FirstName = "Bob",
            LastName  = "Jones"
        };
        _authRepo.Setup(r => r.GetByIdAsync("user-42")).ReturnsAsync(user);

        // Act
        var result = await CreateHandler().Handle(new GetUserByIdQuery("user-42"), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be("user-42");
        result.Email.Should().Be("bob@example.com");
        result.FullName.Should().Be("Bob Jones");
    }

    [Fact]
    public async Task Handle_NonExistentUserId_ReturnsNull()
    {
        // Arrange
        _authRepo.Setup(r => r.GetByIdAsync(It.IsAny<string>()))
                 .ReturnsAsync((ApplicationUser?)null);

        // Act
        var result = await CreateHandler().Handle(new GetUserByIdQuery("missing-id"), CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_PassesCorrectIdToRepository()
    {
        // Arrange
        _authRepo.Setup(r => r.GetByIdAsync("specific-id"))
                 .ReturnsAsync(new ApplicationUser { Id = "specific-id" });

        // Act
        await CreateHandler().Handle(new GetUserByIdQuery("specific-id"), CancellationToken.None);

        // Assert
        _authRepo.Verify(r => r.GetByIdAsync("specific-id"), Times.Once);
    }

    [Fact]
    public async Task Handle_UserWithEmptyNames_ReturnsEmptyFullName()
    {
        // Arrange
        var user = new ApplicationUser { Id = "u1", FirstName = "", LastName = "" };
        _authRepo.Setup(r => r.GetByIdAsync("u1")).ReturnsAsync(user);

        // Act
        var result = await CreateHandler().Handle(new GetUserByIdQuery("u1"), CancellationToken.None);

        // Assert
        result!.FullName.Should().BeEmpty();
    }
}
