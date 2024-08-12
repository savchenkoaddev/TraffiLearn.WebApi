using FluentValidation;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Application.CustomValidators
{
    internal sealed class EmailValidator
        : AbstractValidator<string?>
    {
        public EmailValidator()
        {
            RuleFor(email => email)
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Email is not in the proper format.")
                .MaximumLength(Email.MaxLength)
                .NotEmpty();
        }
    }
}
