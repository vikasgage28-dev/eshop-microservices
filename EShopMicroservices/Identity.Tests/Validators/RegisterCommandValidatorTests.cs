using FluentAssertions;
using FluentValidation.TestHelper;
using Identity.Core.Features.Auth.Commands.Register;

namespace Identity.Tests.Validators;

public class RegisterCommandValidatorTests
{
    private readonly RegisterCommandValidator _validator = new();

    private static RegisterCommand ValidCommand() => new()
    {
        FirstName = "Jane",
        LastName  = "Doe",
        Email     = "jane@example.com",
        Password  = "SecurePass@1",
        Role      = "Customer"
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
    public void Validate_EmptyFirstName_FailsValidation(string firstName)
    {
        var cmd = ValidCommand(); cmd.FirstName = firstName;
        var result = _validator.TestValidate(cmd);
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }

    [Fact]
    public void Validate_EmptyEmail_FailsValidation()
    {
        var cmd = ValidCommand(); cmd.Email = "";
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

    [Fact]
    public void Validate_PasswordTooShort_FailsValidation()
    {
        var cmd = ValidCommand(); cmd.Password = "Ab1";
        var result = _validator.TestValidate(cmd);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Validate_PasswordWithoutUppercase_FailsValidation()
    {
        var cmd = ValidCommand(); cmd.Password = "lowercase1";
        var result = _validator.TestValidate(cmd);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Validate_PasswordWithoutDigit_FailsValidation()
    {
        var cmd = ValidCommand(); cmd.Password = "NoDigitHere";
        var result = _validator.TestValidate(cmd);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Theory]
    [InlineData("Manager")]
    [InlineData("Guest")]
    [InlineData("")]
    public void Validate_InvalidRole_FailsValidation(string role)
    {
        var cmd = ValidCommand(); cmd.Role = role;
        var result = _validator.TestValidate(cmd);
        result.ShouldHaveValidationErrorFor(x => x.Role);
    }

    [Theory]
    [InlineData("Customer")]
    [InlineData("Admin")]
    public void Validate_ValidRoles_PassesValidation(string role)
    {
        var cmd = ValidCommand(); cmd.Role = role;
        var result = _validator.TestValidate(cmd);
        result.ShouldNotHaveValidationErrorFor(x => x.Role);
    }
}
