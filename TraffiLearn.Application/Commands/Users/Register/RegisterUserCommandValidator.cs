using FluentValidation;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Application.Commands.Users.Register
{
    internal sealed class RegisterUserCommandValidator 
        : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .MaximumLength(Email.MaxLength)
                .NotEmpty();

            RuleFor(x => x.Password)
                .MinimumLength(8)
                .MaximumLength(30)
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"[0-9]").WithMessage("Password must contain at least one number.")
                .Matches(@"[\W_]").WithMessage("Password must contain at least one special character.")
                .NotEmpty();

            RuleFor(x => x.Username)
                .MaximumLength(Username.MaxLength)
                .NotEmpty();
        }
    }
}
