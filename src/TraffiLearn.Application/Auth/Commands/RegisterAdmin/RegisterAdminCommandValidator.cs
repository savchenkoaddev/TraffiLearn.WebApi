using FluentValidation;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;

namespace TraffiLearn.Application.Auth.Commands.RegisterAdmin
{
    internal sealed class RegisterAdminCommandValidator
        : AbstractValidator<RegisterAdminCommand>
    {
        public RegisterAdminCommandValidator()
        {
            RuleFor(x => x.Email)
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Email is not in the proper format.")
                .MaximumLength(Email.MaxLength)
                .NotEmpty();

            RuleFor(x => x.Password)
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .MaximumLength(30).WithMessage("Password must be no more than 30 characters long.")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"[0-9]").WithMessage("Password must contain at least one number.")
                .Matches(@"[\W_]").WithMessage("Password must contain at least one special character.")
                .NotEmpty().WithMessage("Password cannot be empty.");

            RuleFor(x => x.Username)
                .MaximumLength(Username.MaxLength)
                .NotEmpty();
        }
    }
}
