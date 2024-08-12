using FluentValidation;

namespace TraffiLearn.Application.Commands.Users.DowngradeAccount
{
    internal sealed class DowngradeAccountCommandValidator
        : AbstractValidator<DowngradeAccountCommand>
    {
        public DowngradeAccountCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();
        }
    }
}
