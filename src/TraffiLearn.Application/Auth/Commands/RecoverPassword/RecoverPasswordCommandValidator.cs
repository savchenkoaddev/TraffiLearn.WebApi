using FluentValidation;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;

namespace TraffiLearn.Application.Auth.Commands.RecoverPassword
{
    internal sealed class RecoverPasswordCommandValidator
        : AbstractValidator<RecoverPasswordCommand>
    {
        public RecoverPasswordCommandValidator()
        {
            RuleFor(x => x.Email)
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")
                .MaximumLength(Email.MaxLength)
                .NotEmpty();
        }
    }
}
