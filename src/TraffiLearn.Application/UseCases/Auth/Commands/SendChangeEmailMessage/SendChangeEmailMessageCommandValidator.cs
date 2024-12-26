using FluentValidation;
using TraffiLearn.Domain.Users.Emails;

namespace TraffiLearn.Application.UseCases.Auth.Commands.SendChangeEmailMessage
{
    internal sealed class SendChangeEmailMessageCommandValidator
        : AbstractValidator<SendChangeEmailMessageCommand>
    {
        public SendChangeEmailMessageCommandValidator()
        {
            RuleFor(x => x.NewEmail)
                .NotEmpty();

            RuleFor(x => x.NewEmail)
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Email is not in the proper format.")
                .MaximumLength(Email.MaxLength);
        }
    }
}
