using FluentValidation;
using TraffiLearn.Application.CustomValidators;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Application.Commands.Auth.RegisterAdmin
{
    internal sealed class RegisterAdminCommandValidator
        : AbstractValidator<RegisterAdminCommand>
    {
        public RegisterAdminCommandValidator()
        {
            RuleFor(x => x.Email)
                .SetValidator(new EmailValidator());

            RuleFor(x => x.Password)
                .SetValidator(new PasswordValidator());

            RuleFor(x => x.Username)
                .MaximumLength(Username.MaxLength)
                .NotEmpty();
        }
    }
}
