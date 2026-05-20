using FluentAssertions;
using Identity.Core.Entities;
using Identity.Core.Features.Auth.Commands.Register;
using Identity.Core.Interfaces;
using Moq;

namespace Identity.Tests.Handlers;

public class RegisterCommandHandlerTests
{
    private readonly Mock<IAuthRepository> _authRepo = new();
    private readonly Mock<ITokenService>   _tokenSvc = new();

    private RegisterCommandHandler CreateHandler() =>
        new(_authRepo.Object, _tokenSvc.Object);

    private static RegisterCommand ValidCommand() => new()
    {
        FirstName = "John",
        LastName  = "Doe",
        Email     = "john@example.com",
        Password  = "Pass@1234",
        Role      = "Customer"
    };

    [Fact]
    public async Task Handle_ValidRequest_ReturnsSuccessWithTokens()
    {
        // Arrange
        _authRepo.Setup(r => r.RegisterAsync(It.IsAny<ApplicationUser>(), "Pass@1234", "Customer"))
                 .ReturnsAsync((true, (string?)null))
                 .Callback<ApplicationUser, string, string>((u, _, _) => u.Id = "user-123");

        _authRepo.Setup(r => r.GetRolesAsync(It.IsAny<ApplicationUser>()))
                 .ReturnsAsync(new List<string> { "Customer" });

        _authRepo.Setup(r => r.UpdateRefreshTokenAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                 .Returns(Task.CompletedTask);

        _tokenSvc.Setup(t => t.GenerateAccessToken(It.IsAny<ApplicationUser>(), It.IsAny<IList<string>>()))
                 .Returns("access-token");

        _tokenSvc.Setup(t => t.GenerateRefreshToken())
                 .Returns("refresh-token");

        // Act
        var result = await CreateHandler().Handle(ValidCommand(), CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.Token.Should().Be("access-token");
        result.RefreshToken.Should().Be("refresh-token");
        result.Error.Should().BeNull();
    }

    [Fact]
    public async Task Handle_RegistrationFails_ReturnsFailureWithError()
    {
        // Arrange
        _authRepo.Setup(r => r.RegisterAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()))
                 .ReturnsAsync((false, "Email already exists."));

        // Act
        var result = await CreateHandler().Handle(ValidCommand(), CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.Error.Should().Be("Email already exists.");
        result.Token.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ValidRequest_CallsUpdateRefreshToken()
    {
        // Arrange
        _authRepo.Setup(r => r.RegisterAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()))
                 .ReturnsAsync((true, (string?)null));
        _authRepo.Setup(r => r.GetRolesAsync(It.IsAny<ApplicationUser>()))
                 .ReturnsAsync(new List<string>());
        _authRepo.Setup(r => r.UpdateRefreshTokenAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                 .Returns(Task.CompletedTask);
        _tokenSvc.Setup(t => t.GenerateAccessToken(It.IsAny<ApplicationUser>(), It.IsAny<IList<string>>()))
                 .Returns("token");
        _tokenSvc.Setup(t => t.GenerateRefreshToken()).Returns("refresh");

        // Act
        await CreateHandler().Handle(ValidCommand(), CancellationToken.None);

        // Assert
        _authRepo.Verify(r => r.UpdateRefreshTokenAsync(It.IsAny<string>(), "refresh", It.IsAny<DateTime>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ValidRequest_GeneratesAccessToken()
    {
        // Arrange
        _authRepo.Setup(r => r.RegisterAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()))
                 .ReturnsAsync((true, (string?)null));
        _authRepo.Setup(r => r.GetRolesAsync(It.IsAny<ApplicationUser>()))
                 .ReturnsAsync(new List<string> { "Customer" });
        _authRepo.Setup(r => r.UpdateRefreshTokenAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                 .Returns(Task.CompletedTask);
        _tokenSvc.Setup(t => t.GenerateAccessToken(It.IsAny<ApplicationUser>(), It.IsAny<IList<string>>()))
                 .Returns("generated-token");
        _tokenSvc.Setup(t => t.GenerateRefreshToken()).Returns("r");

        // Act
        var result = await CreateHandler().Handle(ValidCommand(), CancellationToken.None);

        // Assert
        _tokenSvc.Verify(t => t.GenerateAccessToken(It.IsAny<ApplicationUser>(), It.IsAny<IList<string>>()), Times.Once);
        result.Token.Should().Be("generated-token");
    }
}
