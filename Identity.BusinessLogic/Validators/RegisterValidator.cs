using FluentValidation;
using Identity.BusinessLogic.DTOs;

namespace Identity.BusinessLogic.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterDTO>
    {
        public RegisterValidator()
        {
            RuleFor(r => r.Username)
                .MaximumLength(20).WithMessage("Username cannot be longer than 20 characters")
                .NotEmpty().WithMessage("Username field should not be empty");

            RuleFor(r => r.Email)
                .EmailAddress()
                .NotEmpty().WithMessage("Email field should not be empty");

            RuleFor(r => r.Password)
                .NotEmpty().WithMessage("Password field should not be empty")
                    .MinimumLength(8).WithMessage("Password field must be at least 8")
                    .MaximumLength(16).WithMessage("Password field must not exceed 16")
                    .Matches(@"[A-Z]+").WithMessage("Password field must contain at least one uppercase letter")
                    .Matches(@"[a-z]+").WithMessage("Password field must contain at least one lowercase letter")
                    .Matches(@"[0-9]+").WithMessage("Password field must contain at least one digit")
                    .Matches(@"[\!\?\*\.]+").WithMessage("Password field must contain at least one special character");

            RuleFor(r => r.ConfirmPassword)
                .Equal(r => r.Password).WithMessage("This field must match to Password");
        }
    }
}
