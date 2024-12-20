using FluentValidation;
using TraffiLearn.Domain.Users.Emails;

namespace TraffiLearn.Application.UseCases.Auth.Commands.ResendConfirmationEmail
{
    internal sealed class ResendConfirmationEmailCommandValidator
        : AbstractValidator<ResendConfirmationEmailCommand>
    {
        public ResendConfirmationEmailCommandValidator()
        {
            RuleFor(x => x.Email)
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Email is not in the proper format.")
                .MaximumLength(Email.MaxLength)
                .NotEmpty();
        }
    }
}
