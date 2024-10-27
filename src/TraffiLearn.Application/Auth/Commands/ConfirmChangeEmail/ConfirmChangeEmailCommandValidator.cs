using FluentValidation;

namespace TraffiLearn.Application.Auth.Commands.ConfirmChangeEmail
{
    internal sealed class ConfirmChangeEmailCommandValidator
        : AbstractValidator<ConfirmChangeEmailCommand>
    {
        public ConfirmChangeEmailCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.Token)
                .NotEmpty();

            RuleFor(x => x.NewEmail)
                .NotEmpty();
        }
    }
}
