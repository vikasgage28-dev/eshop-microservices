using FluentAssertions;
using Identity.Core.Entities;
using Identity.Core.Features.Auth.Commands.Login;
using Identity.Core.Interfaces;
using Moq;

namespace Identity.Tests.Handlers;

public class LoginCommandHandlerTests
{
    private readonly Mock<IAuthRepository> _authRepo = new();
    private readonly Mock<ITokenService>   _tokenSvc = new();

    private LoginCommandHandler CreateHandler() =>
        new(_authRepo.Object, _tokenSvc.Object);

    private static ApplicationUser FakeUser() => new()
    {
        Id        = "user-1",
        Email     = "alice@example.com",
        FirstName = "Alice",
        LastName  = "Smith"
    };

    [Fact]
    public async Task Handle_ValidCredentials_ReturnsSuccessWithTokens()
    {
        // Arrange
        var user = FakeUser();
        _authRepo.Setup(r => r.ValidateCredentialsAsync("alice@example.com", "Pass@1"))
                 .ReturnsAsync(user);
        _authRepo.Setup(r => r.GetRolesAsync(user))
                 .ReturnsAsync(new List<string> { "Customer" });
        _authRepo.Setup(r => r.UpdateRefreshTokenAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                 .Returns(Task.CompletedTask);
        _tokenSvc.Setup(t => t.GenerateAccessToken(user, It.IsAny<IList<string>>()))
                 .Returns("access-token");
        _tokenSvc.Setup(t => t.GenerateRefreshToken())
                 .Returns("refresh-token");

        var cmd = new LoginCommand { Email = "alice@example.com", Password = "Pass@1" };

        // Act
        var result = await CreateHandler().Handle(cmd, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.Token.Should().Be("access-token");
        result.RefreshToken.Should().Be("refresh-token");
        result.UserId.Should().Be("user-1");
        result.Email.Should().Be("alice@example.com");
    }

    [Fact]
    public async Task Handle_InvalidCredentials_ReturnsFailure()
    {
        // Arrange
        _authRepo.Setup(r => r.ValidateCredentialsAsync(It.IsAny<string>(), It.IsAny<string>()))
                 .ReturnsAsync((ApplicationUser?)null);

        var cmd = new LoginCommand { Email = "bad@example.com", Password = "wrong" };

        // Act
        var result = await CreateHandler().Handle(cmd, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.Error.Should().Be("Invalid email or password.");
        result.Token.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ValidCredentials_IncludesRolesInResult()
    {
        // Arrange
        var user = FakeUser();
        _authRepo.Setup(r => r.ValidateCredentialsAsync(It.IsAny<string>(), It.IsAny<string>()))
                 .ReturnsAsync(user);
        _authRepo.Setup(r => r.GetRolesAsync(user))
                 .ReturnsAsync(new List<string> { "Admin", "Customer" });
        _authRepo.Setup(r => r.UpdateRefreshTokenAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                 .Returns(Task.CompletedTask);
        _tokenSvc.Setup(t => t.GenerateAccessToken(It.IsAny<ApplicationUser>(), It.IsAny<IList<string>>()))
                 .Returns("token");
        _tokenSvc.Setup(t => t.GenerateRefreshToken()).Returns("refresh");

        var cmd = new LoginCommand { Email = "alice@example.com", Password = "Pass@1" };

        // Act
        var result = await CreateHandler().Handle(cmd, CancellationToken.None);

        // Assert
        result.Roles.Should().BeEquivalentTo(new[] { "Admin", "Customer" });
    }

    [Fact]
    public async Task Handle_ValidCredentials_StoresRefreshToken()
    {
        // Arrange
        var user = FakeUser();
        _authRepo.Setup(r => r.ValidateCredentialsAsync(It.IsAny<string>(), It.IsAny<string>()))
                 .ReturnsAsync(user);
        _authRepo.Setup(r => r.GetRolesAsync(user)).ReturnsAsync(new List<string>());
        _authRepo.Setup(r => r.UpdateRefreshTokenAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                 .Returns(Task.CompletedTask);
        _tokenSvc.Setup(t => t.GenerateAccessToken(It.IsAny<ApplicationUser>(), It.IsAny<IList<string>>()))
                 .Returns("token");
        _tokenSvc.Setup(t => t.GenerateRefreshToken()).Returns("stored-refresh");

        var cmd = new LoginCommand { Email = "alice@example.com", Password = "Pass@1" };

        // Act
        await CreateHandler().Handle(cmd, CancellationToken.None);

        // Assert
        _authRepo.Verify(r => r.UpdateRefreshTokenAsync("user-1", "stored-refresh", It.IsAny<DateTime>()), Times.Once);
    }
}
