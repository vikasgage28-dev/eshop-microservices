using FluentAssertions;
using FluentValidation.TestHelper;
using Identity.Core.Features.Auth.Commands.Login;

namespace Identity.Tests.Validators;

public class LoginCommandValidatorTests
{
    private readonly LoginCommandValidator _validator = new();

    private static LoginCommand ValidCommand() => new()
    {
        Email    = "user@example.com",
        Password = "Pass@1234"
    };

    [Fact]
    public void Validate_ValidCommand_PassesValidation()
    {
        var result = _validator.TestValidate(ValidCommand());
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_EmptyEmail_FailsValidation(string email)
    {
        var cmd = ValidCommand(); cmd.Email = email;
        var result = _validator.TestValidate(cmd);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Validate_InvalidEmailFormat_FailsValidation()
    {
        var cmd = ValidCommand(); cmd.Email = "not-an-email";
        var result = _validator.TestValidate(cmd);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_EmptyPassword_FailsValidation(string password)
    {
        var cmd = ValidCommand(); cmd.Password = password;
        var result = _validator.TestValidate(cmd);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Validate_ValidEmailAndPassword_BothPass()
    {
        var cmd = new LoginCommand { Email = "admin@eshop.com", Password = "Admin@12345" };
        var result = _validator.TestValidate(cmd);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
