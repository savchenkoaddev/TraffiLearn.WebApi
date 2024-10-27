using FluentValidation;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;

namespace TraffiLearn.Application.Auth.Commands.SendRecoverPasswordMessage
{
    internal sealed class SendRecoverPasswordMessageCommandValidator
        : AbstractValidator<SendRecoverPasswordMessageCommand>
    {
        public SendRecoverPasswordMessageCommandValidator()
        {
            RuleFor(x => x.Email)
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")
                .MaximumLength(Email.MaxLength)
                .NotEmpty();
        }
    }
}
