using FluentAssertions;
using Identity.Core.Entities;
using Identity.Core.Features.Auth.Commands.RefreshToken;
using Identity.Core.Interfaces;
using Moq;

namespace Identity.Tests.Handlers;

public class RefreshTokenCommandHandlerTests
{
    private readonly Mock<IAuthRepository> _authRepo = new();
    private readonly Mock<ITokenService>   _tokenSvc = new();

    private RefreshTokenCommandHandler CreateHandler() =>
        new(_authRepo.Object, _tokenSvc.Object);

    private static ApplicationUser FakeUser(DateTime? expiry = null) => new()
    {
        Id                  = "user-1",
        Email               = "alice@example.com",
        FirstName           = "Alice",
        LastName            = "Smith",
        RefreshToken        = "valid-refresh",
        RefreshTokenExpiry  = expiry ?? DateTime.UtcNow.AddDays(7)
    };

    [Fact]
    public async Task Handle_ValidRefreshToken_ReturnsNewTokens()
    {
        // Arrange
        var user = FakeUser();
        _authRepo.Setup(r => r.GetByRefreshTokenAsync("valid-refresh"))
                 .ReturnsAsync(user);
        _authRepo.Setup(r => r.GetRolesAsync(user))
                 .ReturnsAsync(new List<string> { "Customer" });
        _authRepo.Setup(r => r.UpdateRefreshTokenAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                 .Returns(Task.CompletedTask);
        _tokenSvc.Setup(t => t.GenerateAccessToken(user, It.IsAny<IList<string>>()))
                 .Returns("new-access-token");
        _tokenSvc.Setup(t => t.GenerateRefreshToken())
                 .Returns("new-refresh-token");

        var cmd = new RefreshTokenCommand { RefreshToken = "valid-refresh" };

        // Act
        var result = await CreateHandler().Handle(cmd, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.Token.Should().Be("new-access-token");
        result.RefreshToken.Should().Be("new-refresh-token");
        result.Error.Should().BeNull();
    }

    [Fact]
    public async Task Handle_RefreshTokenNotFound_ReturnsFailure()
    {
        // Arrange
        _authRepo.Setup(r => r.GetByRefreshTokenAsync(It.IsAny<string>()))
                 .ReturnsAsync((ApplicationUser?)null);

        var cmd = new RefreshTokenCommand { RefreshToken = "unknown-token" };

        // Act
        var result = await CreateHandler().Handle(cmd, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.Error.Should().Be("Invalid or expired refresh token.");
    }

    [Fact]
    public async Task Handle_ExpiredRefreshToken_ReturnsFailure()
    {
        // Arrange
        var user = FakeUser(expiry: DateTime.UtcNow.AddDays(-1)); // expired
        _authRepo.Setup(r => r.GetByRefreshTokenAsync(It.IsAny<string>()))
                 .ReturnsAsync(user);

        var cmd = new RefreshTokenCommand { RefreshToken = "expired-token" };

        // Act
        var result = await CreateHandler().Handle(cmd, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.Error.Should().Be("Invalid or expired refresh token.");
    }

    [Fact]
    public async Task Handle_ValidRefreshToken_UpdatesStoredRefreshToken()
    {
        // Arrange
        var user = FakeUser();
        _authRepo.Setup(r => r.GetByRefreshTokenAsync(It.IsAny<string>())).ReturnsAsync(user);
        _authRepo.Setup(r => r.GetRolesAsync(user)).ReturnsAsync(new List<string>());
        _authRepo.Setup(r => r.UpdateRefreshTokenAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                 .Returns(Task.CompletedTask);
        _tokenSvc.Setup(t => t.GenerateAccessToken(It.IsAny<ApplicationUser>(), It.IsAny<IList<string>>()))
                 .Returns("t");
        _tokenSvc.Setup(t => t.GenerateRefreshToken()).Returns("new-refresh");

        // Act
        await CreateHandler().Handle(new RefreshTokenCommand { RefreshToken = "old" }, CancellationToken.None);

        // Assert
        _authRepo.Verify(r => r.UpdateRefreshTokenAsync("user-1", "new-refresh", It.IsAny<DateTime>()), Times.Once);
    }
}
