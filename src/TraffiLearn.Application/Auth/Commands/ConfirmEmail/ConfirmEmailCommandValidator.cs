using FluentValidation;

namespace TraffiLearn.Application.Auth.Commands.ConfirmEmail
{
    internal sealed class ConfirmEmailCommandValidator
        : AbstractValidator<ConfirmEmailCommand>
    {
        public ConfirmEmailCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.Token)
                .NotEmpty();
        }
    }
}
