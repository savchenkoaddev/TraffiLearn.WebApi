using FluentValidation;
using TraffiLearn.Domain.Users.Emails;

namespace TraffiLearn.Application.UseCases.Auth.Commands.ConfirmChangeEmail
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
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Email is not in the proper format.")
                .MaximumLength(Email.MaxLength)
                .NotEmpty();
        }
    }
}
