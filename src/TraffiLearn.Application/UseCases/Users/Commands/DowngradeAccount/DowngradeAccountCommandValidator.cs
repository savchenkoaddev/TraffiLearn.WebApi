using FluentValidation;

namespace TraffiLearn.Application.UseCases.Users.Commands.DowngradeAccount
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
