using FluentValidation;

namespace Customer.Core.Features.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(100).WithMessage("First name must not exceed 100 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(100).WithMessage("Last name must not exceed 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.")
                .MaximumLength(200).WithMessage("Email must not exceed 200 characters.");

            RuleFor(x => x.Phone)
                .MaximumLength(20).WithMessage("Phone must not exceed 20 characters.")
                .When(x => x.Phone is not null);
        }
    }
}
